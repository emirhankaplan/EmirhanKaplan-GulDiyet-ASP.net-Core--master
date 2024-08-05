using Microsoft.Extensions.DependencyInjection;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.Services;

namespace GulDiyet.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddTransient<IDiyetisyenService, DiyetisyenService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<ILaboratoryTestService, LaboratoryTestService>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<ILaboratoryResultService, LaboratoryResultService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAppointmentFeedbackService, AppointmentFeedbackService>();
            services.AddTransient<IAppointmentPeriodService, AppointmentPeriodService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IDietPlanService, DietPlanService>();
            services.AddTransient<IEvaluationService, EvaluationService>();
            services.AddTransient<IPdfReportService, PdfReportService>();

            // PDF Service
            services.AddTransient<IPdfService, PdfService>();

            // RabbitMQ Service Singleton
            services.AddSingleton<IRabbitMQService, RabbitMQService>();

            // SignalR Hub
            services.AddSignalR();
        }
    }
}
