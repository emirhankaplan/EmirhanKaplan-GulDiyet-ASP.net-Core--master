using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.Interfaces.Repositories
{
    public interface IPatientRepository : IGenericRepository<Patient>
    {
        Task<List<Appointment>> GetAppointmentsByPatientIdAsync(int patientId); // Eksik olan yöntem

    }
}
