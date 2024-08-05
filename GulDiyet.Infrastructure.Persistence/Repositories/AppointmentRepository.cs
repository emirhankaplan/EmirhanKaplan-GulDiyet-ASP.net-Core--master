using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Email;
using GulDiyet.Core.Domain.Entities;
using GulDiyet.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GulDiyet.Infrastructure.Persistence.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public AppointmentRepository(ApplicationDbContext dbContext, IServiceProvider serviceProvider) : base(dbContext)
        {
            this._dbContext = dbContext;
            this._serviceProvider = serviceProvider;
        }

        public async Task<Appointment?> GetByIdWithIncludeAsync(int id, List<string> properties)
        {
            var query = _dbContext.Set<Appointment>().AsQueryable();

            foreach (string property in properties)
            {
                query = query.Include(property);
            }

            return await query.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task SendAppointmentNotificationAsync(int appointmentId, string message)
        {
            var appointment = await GetByIdAsync(appointmentId);
            if (appointment != null)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var rabbitMQService = scope.ServiceProvider.GetRequiredService<IRabbitMQService>();
                    rabbitMQService.SendMessage(new SaveEmailViewModel
                    {
                        To = appointment.Patient.Email,
                        Subject = "Appointment Notification",
                        Body = message
                    });
                }
            }
        }
    }
}
