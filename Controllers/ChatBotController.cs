using Microsoft.AspNetCore.Mvc;
using OpenAI;
using System.Text.Json;

namespace UsersApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBotController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly OpenAIClient _openAIClient;

        public ChatBotController(IConfiguration configuration)
        {
            _configuration = configuration;
            var apiKey = _configuration["OpenAI:ApiKey"] ?? "";
            _openAIClient = new OpenAIClient(apiKey);
        }

        [HttpPost("chat")]
        public IActionResult Chat([FromBody] ChatRequest request)
        {
            try
            {
                // Simple response for now - you can integrate with OpenAI API later
                var response = GetSimpleMedicalResponse(request.Message);

                return Ok(new ChatResponse
                {
                    Message = response,
                    Timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while processing the message", details = ex.Message });
            }
        }

        private string GetSimpleMedicalResponse(string userMessage)
        {
            var message = userMessage.ToLower();
            
            if (message.Contains("ألم") || message.Contains("وجع"))
            {
                return "If you are experiencing pain, please specify the location and intensity. For severe or sudden pain, please consult a doctor immediately.";
            }
            else if (message.Contains("حمى") || message.Contains("سخونة"))
            {
                return "The fever may be a sign of an infection. If it persists for more than two days or is severe, please consult a doctor.";
            }
            else if (message.Contains("صداع"))
            {
                return "Headache may be due to stress or dehydration. Rest and drink water. If it is severe or recurrent, consult a doctor.";
            }
            else if (message.Contains("سعال") || message.Contains("كحة"))
            {
                return "Cough may be due to a cold or sensitivity. If it persists for more than a week or is accompanied by a fever, consult a doctor.";
            }
            else if (message.Contains("ضغط") || message.Contains("دموي"))
            {
                return "High blood pressure requires regular monitoring. Please consult a doctor for necessary tests.";
            }
            else
            {
                return "Hello! I am a smart medical assistant. I can provide general advice, but for severe or persistent symptoms, please consult a specialist. How can I help you?";
            }
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", timestamp = DateTime.Now });
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }

    public class ChatResponse
    {
        public string Message { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
} 