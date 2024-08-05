using GulDiyet.Infrastructure.Persistence;
using GulDiyet.Core.Application;
using GulDiyet.Middlewares;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using GulDiyet.Core.Domain.Entities;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.Services;
using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession();
builder.Services.AddPersistenceLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddControllersWithViews();

// Configuration for RabbitMQ and SMTP
builder.Services.Configure<GulDiyet.Core.Domain.Entities.RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQConfiguration"));
builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("SmtpConfig"));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ValidateUserSession, ValidateUserSession>();

// MassTransit and RabbitMQ configurasyonlarý
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();
builder.Services.AddScoped<IEvaluationService, EvaluationService>(); // Ekleyin
builder.Services.AddScoped<IAppointmentService, AppointmentService>(); // Ekleyin
builder.Services.AddScoped<IPatientService, PatientService>(); // Ekleyin
builder.Services.AddScoped<IDiyetisyenService, DiyetisyenService>(); // Ekleyin
builder.Services.AddScoped<ILaboratoryResultService, LaboratoryResultService>(); // Ekleyin
builder.Services.AddScoped<ILaboratoryTestService, LaboratoryTestService>(); // Ekleyin
builder.Services.AddScoped<IPdfReportService, PdfReportService>(); // Ekleyin

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
