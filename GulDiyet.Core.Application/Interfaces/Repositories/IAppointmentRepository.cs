using GulDiyet.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Interfaces.Repositories
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<Appointment?> GetByIdWithIncludeAsync(int id, List<string> properties);
        Task SendAppointmentNotificationAsync(int appointmentId, string message);
    }
}
