using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.Services;
using GulDiyet.Core.Domain.Entities;
using GulDiyet.Infrastructure.Persistence.Contexts;
using GulDiyet.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace GulDiyet.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration builder)
        {
            #region "Context configurations"
            if (builder.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("AppDb"));
            }
            else
            {
                var connectionString = builder.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationDbContext>(options =>
                                            options.UseSqlServer(connectionString,
                                            m => m.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IDiyetisyenRepository, DiyetisyenRepository>();
            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<ILaboratoryTestRepository, LaboratoryTestRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();
            services.AddTransient<ILaboratoryResultRepository, LaboratoryResultRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IEmailRepository, EmailRepository>();
            services.AddTransient<IDietPlanRepository, DietPlanRepository>();
            services.AddTransient<IEvaluationRepository, EvaluationRepository>();
            services.AddTransient<ITimeSlotRepository, TimeSlotRepository>();
            #endregion

            #region RabbitMQ
            var rabbitMqSettings = builder.GetSection("RabbitMQConfiguration").Get<RabbitMQConfiguration>();
            services.AddSingleton(rabbitMqSettings);

            var factory = new ConnectionFactory()
            {
                HostName = rabbitMqSettings.HostName,
                UserName = rabbitMqSettings.UserName,
                Password = rabbitMqSettings.Password
            };

            services.AddSingleton(factory);
            services.AddScoped<IRabbitMQService, RabbitMQService>();
            #endregion

            #region SignalR
            services.AddSignalR();
            #endregion
        }
    }
}
