using GulDiyet.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Interfaces.Repositories
{
    public interface IDietPlanRepository : IGenericRepository<DietPlan>
    {
        Task<List<DietPlan>> GetDietPlansByPatientIdAsync(int patientId);
        Task<List<DietPlan>> GetDietPlansByDiyetisyenIdAsync(int diyetisyenId); // Eksik olan yöntem
    }
}
