# نظام إدارة الرعاية الصحية 🏥

نظام متكامل لإدارة العيادات الطبية والمواعيد والمرضى، مبني باستخدام ASP.NET Core MVC مع دعم كامل للغة العربية.

## ✨ الميزات الرئيسية

### 👨‍⚕️ لوحة تحكم المدير (Admin Dashboard)
- **إدارة العيادات**: إضافة، تعديل، وعرض العيادات الطبية
- **إدارة الأطباء**: إضافة أطباء جدد مع تخصصاتهم
- **إدارة المواعيد**: عرض وتحديث حالة جميع المواعيد
- **إحصائيات شاملة**: عدد المرضى، الأطباء، العيادات، والمواعيد

### 👤 لوحة تحكم المريض (Patient Dashboard)
- **استعراض الأطباء**: عرض قائمة الأطباء المتاحين مع تفاصيلهم
- **استعراض العيادات**: عرض العيادات المتاحة وخدماتها
- **حجز المواعيد**: حجز مواعيد مع الأطباء المختصين
- **إدارة المواعيد**: عرض وإلغاء المواعيد الشخصية
- **الاستمارات الطبية**: إنشاء وتحميل استمارات طبية كـ PDF
- **المساعد الطبي الذكي**: ChatBot للإجابة على الاستفسارات الصحية

### 🔐 نظام المصادقة والتفويض
- **تسجيل الدخول والخروج**: نظام مصادقة آمن
- **تسجيل حساب جديد**: مع اختيار الدور (مدير/مريض)
- **نظام الأدوار**: صلاحيات مختلفة للمدير والمريض
- **تسجيل الدخول بحساب Google**: دعم المصادقة الخارجية

## 🛠️ التقنيات المستخدمة

- **ASP.NET Core 8.0**: إطار العمل الرئيسي
- **Entity Framework Core**: ORM لقاعدة البيانات
- **SQL Server**: قاعدة البيانات
- **ASP.NET Core Identity**: نظام المصادقة والتفويض
- **Bootstrap 4**: واجهة المستخدم
- **jQuery**: التفاعل في الواجهة الأمامية
- **Font Awesome**: الأيقونات
- **RTL Support**: دعم اللغة العربية والاتجاه من اليمين لليسار

## 📋 متطلبات النظام

- .NET 8.0 SDK
- SQL Server (LocalDB أو Express أو Full)
- Visual Studio 2022 أو VS Code

## 🚀 كيفية التثبيت والتشغيل

### 1. استنساخ المشروع
```bash
git clone <repository-url>
cd Capstone_Final
```

### 2. تثبيت الحزم المطلوبة
```bash
dotnet restore
```

### 3. تحديث قاعدة البيانات
```bash
dotnet ef database update
```

### 4. تشغيل المشروع
```bash
dotnet run
```

### 5. الوصول للتطبيق
افتح المتصفح وانتقل إلى:
- **الرئيسية**: `https://localhost:5001`
- **تسجيل الدخول**: `https://localhost:5001/Account/Login`

## 👥 بيانات الاختبار

تم إعداد بيانات اختبار تلقائياً:

### حساب المدير
- **البريد الإلكتروني**: `admin@test.com`
- **كلمة المرور**: `Admin123!`

### حساب المريض
- **البريد الإلكتروني**: `patient@test.com`
- **كلمة المرور**: `Patient123!`

### بيانات العيادات والأطباء
- **3 عيادات** مختلفة التخصصات
- **3 أطباء** متخصصين في مجالات مختلفة

## 📁 هيكل المشروع

```
Capstone_Final/
├── Controllers/          # Controllers
│   ├── AdminController.cs
│   ├── PatientDashboardController.cs
│   ├── AccountController.cs
│   ├── ChatBotController.cs
│   └── HomeController.cs
├── Models/              # Entity Models
│   ├── Users.cs
│   ├── Clinic.cs
│   ├── Doctor.cs
│   ├── Appointment.cs
│   └── MedicalForm.cs
├── ViewModels/          # View Models
│   ├── RegisterViewModel.cs
│   ├── LoginViewModel.cs
│   ├── ClinicViewModel.cs
│   ├── DoctorRegisterViewModel.cs
│   ├── AppointmentViewModel.cs
│   └── MedicalFormViewModel.cs
├── Views/               # Razor Views
│   ├── Admin/          # Admin Views
│   ├── PatientDashboard/ # Patient Views
│   ├── Account/        # Authentication Views
│   ├── Home/           # Home Views
│   └── Shared/         # Shared Views
├── Data/               # Database Context
│   ├── AppDbContext.cs
│   └── DbInitializer.cs
├── wwwroot/            # Static Files
│   ├── css/
│   ├── js/
│   └── lib/
└── README.md           # Documentation
```

## 🔧 الإعدادات

### ملف appsettings.json
```json
{
  "ConnectionStrings": {
    "Default": "Data Source=YOUR_SERVER;Initial Catalog=YOUR_DB;Integrated Security=True;TrustServerCertificate=True;"
  },
  "Authentication": {
    "Google": {
      "ClientId": "YOUR_GOOGLE_CLIENT_ID",
      "ClientSecret": "YOUR_GOOGLE_CLIENT_SECRET"
    }
  },
  "OpenAI": {
    "ApiKey": "YOUR_OPENAI_API_KEY"
  }
}
```

## 🔒 الأمان

- **تشفير كلمات المرور**: باستخدام ASP.NET Core Identity
- **حماية CSRF**: باستخدام AntiForgeryToken
- **تشفير البيانات**: حماية البيانات الحساسة
- **صلاحيات الأدوار**: فصل صلاحيات المدير والمريض

## 🌐 API Endpoints

### ChatBot API
- `POST /api/ChatBot/chat`: إرسال رسالة للمساعد الطبي
- `GET /api/ChatBot/health`: فحص حالة الخدمة

### Admin APIs
- `POST /Admin/UpdateAppointmentStatus`: تحديث حالة الموعد

### Patient APIs
- `POST /PatientDashboard/CancelAppointment`: إلغاء موعد

## 🔄 قاعدة البيانات

### الجداول الرئيسية
- **AspNetUsers**: المستخدمين (مديرين ومرضى)
- **AspNetRoles**: الأدوار (Admin, Patient)
- **Clinics**: العيادات الطبية
- **Doctors**: الأطباء
- **Appointments**: المواعيد
- **MedicalForms**: الاستمارات الطبية

### العلاقات
- كل طبيب ينتمي لعيادة واحدة
- كل موعد مرتبط بمريض وطبيب
- كل استمارة طبية مرتبطة بمريض وطبيب

## 🎨 الواجهة

- **تصميم متجاوب**: يعمل على جميع الأجهزة
- **دعم RTL**: واجهة عربية كاملة
- **ألوان حديثة**: تصميم عصري وجذاب
- **أيقونات Font Awesome**: واجهة غنية بالأيقونات

## 🚀 الميزات المستقبلية

- [ ] دعم المدفوعات الإلكترونية
- [ ] نظام إشعارات متقدم
- [ ] تطبيق موبايل
- [ ] دعم الفيديو كونفرنس
- [ ] نظام تقييم الأطباء
- [ ] دعم التقارير الطبية المتقدمة

## 🤝 المساهمة

1. Fork المشروع
2. إنشاء branch جديد (`git checkout -b feature/AmazingFeature`)
3. Commit التغييرات (`git commit -m 'Add some AmazingFeature'`)
4. Push إلى Branch (`git push origin feature/AmazingFeature`)
5. فتح Pull Request

## 📄 الترخيص

هذا المشروع مرخص تحت رخصة MIT - انظر ملف [LICENSE](LICENSE) للتفاصيل.

## 📞 الدعم

لأي استفسارات أو مشاكل:
- إنشاء Issue في GitHub
- التواصل عبر البريد الإلكتروني

---

**تم تطوير هذا المشروع بواسطة فريق تطوير متخصص في أنظمة الرعاية الصحية** 🏥
