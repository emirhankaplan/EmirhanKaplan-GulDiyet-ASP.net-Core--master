using GulDiyet.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Interfaces.Repositories
{
    public interface IEvaluationRepository : IGenericRepository<Evaluation>
    {
        Task<List<Evaluation>> GetAllWithIncludeAsync(List<string> includes);
        Task<List<Evaluation>> GetEvaluationsByDoctorIdAsync(int doctorId); // Eksik olan yöntem
        Task<List<Evaluation>> GetEvaluationsByAppointmentIdAsync(int appointmentId); // Eksik olan yöntem
    }
}
