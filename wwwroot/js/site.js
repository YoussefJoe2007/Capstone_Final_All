// Site JavaScript Functions

// Initialize tooltips
$(document).ready(function() {
    $('[data-toggle="tooltip"]').tooltip();
    
    // Initialize popovers
    $('[data-toggle="popover"]').popover();
    
    // Auto-hide alerts after 5 seconds
    setTimeout(function() {
        $('.alert').fadeOut('slow');
    }, 5000);
    
    // Smooth scrolling for anchor links
    $('a[href^="#"]').on('click', function(event) {
        var target = $(this.getAttribute('href'));
        if (target.length) {
            event.preventDefault();
            $('html, body').stop().animate({
                scrollTop: target.offset().top
            }, 1000);
        }
    });
});

// ChatBot Functions
function openChatBot() {
    $('#chatBotModal').modal('show');
}

function sendMessage() {
    var message = $('#messageInput').val();
    if (message.trim() === '') return;

    // Add user message to chat
    $('#chatMessages').append('<div class="message user-message"><strong>You:</strong> ' + message + '</div>');
    $('#messageInput').val('');

    // Show loading indicator
    $('#chatMessages').append('<div class="message bot-message"><strong>Assistant:</strong> <i class="fas fa-spinner fa-spin"></i> Typing...</div>');

    // Send to API
    $.ajax({
        url: '/api/ChatBot/chat',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ message: message }),
        success: function(response) {
            // Remove loading message
            $('#chatMessages .message:last-child').remove();
            // Add bot response
            $('#chatMessages').append('<div class="message bot-message"><strong>Assistant:</strong> ' + response.message + '</div>');
            $('#chatMessages').scrollTop($('#chatMessages')[0].scrollHeight);
        },
        error: function() {
            // Remove loading message
            $('#chatMessages .message:last-child').remove();
            // Add error message
            $('#chatMessages').append('<div class="message bot-message text-danger"><strong>Assistant:</strong> Sorry, a connection error occurred</div>');
        }
    });

    $('#chatMessages').scrollTop($('#chatMessages')[0].scrollHeight);
}

// Appointment Functions
function updateAppointmentStatus(appointmentId, status) {
    $.ajax({
        url: '/Admin/UpdateAppointmentStatus',
        type: 'POST',
        data: { appointmentId: appointmentId, status: status },
        success: function(response) {
            if (response.success) {
                showNotification('Appointment status updated successfully', 'success');
            } else {
                showNotification('Error updating appointment status', 'error');
            }
        },
        error: function() {
            showNotification('Connection error', 'error');
        }
    });
}

function cancelAppointment(appointmentId) {
    if (confirm('هل أنت متأكد من إلغاء هذا الموعد؟')) {
        $.ajax({
            url: '/PatientDashboard/CancelAppointment',
            type: 'POST',
            data: { appointmentId: appointmentId },
            success: function(response) {
                if (response.success) {
                    showNotification(response.message, 'success');
                    location.reload();
                } else {
                    showNotification(response.message, 'error');
                }
            },
            error: function() {
                showNotification('حدث خطأ في الاتصال', 'error');
            }
        });
    }
}

// Utility Functions
function showNotification(message, type) {
    var alertClass = type === 'success' ? 'alert-success' : 'alert-danger';
    var alertHtml = '<div class="alert ' + alertClass + ' alert-dismissible fade show" role="alert">' +
                    message +
                    '<button type="button" class="close" data-dismiss="alert" aria-label="Close">' +
                    '<span aria-hidden="true">&times;</span>' +
                    '</button>' +
                    '</div>';
    
    // Add to page
    $('body').prepend(alertHtml);
    
    // Auto-hide after 5 seconds
    setTimeout(function() {
        $('.alert').fadeOut('slow');
    }, 5000);
}

// Form validation helpers
function validateForm(formId) {
    var form = $('#' + formId);
    if (form.length === 0) return true;
    
    var isValid = true;
    form.find('[required]').each(function() {
        if (!$(this).val()) {
            $(this).addClass('is-invalid');
            isValid = false;
        } else {
            $(this).removeClass('is-invalid');
        }
    });
    
    return isValid;
}

// Date and time helpers
function formatDate(date) {
    return new Date(date).toLocaleDateString('ar-EG');
}

function formatTime(time) {
    return time.substring(0, 5);
}

// Initialize when document is ready
$(document).ready(function() {
    // Handle Enter key in chat input
    $('#messageInput').keypress(function(e) {
        if (e.which == 13) {
            sendMessage();
        }
    });
    
    // Handle form submissions
    $('form').on('submit', function() {
        var formId = $(this).attr('id');
        if (formId && !validateForm(formId)) {
            return false;
        }
    });
    
    // Initialize date pickers
    $('input[type="date"]').each(function() {
        var today = new Date().toISOString().split('T')[0];
        $(this).attr('min', today);
    });
});

