using System.Collections.Generic;
using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.DietPlan;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IDietPlanService
    {
        Task<DietPlanViewModel> CreateDietPlanAsync(SaveDietPlanViewModel saveDietPlanViewModel);
        Task UpdateDietPlanAsync(SaveDietPlanViewModel saveDietPlanViewModel);
        Task DeleteDietPlanAsync(int id);
        Task<DietPlanViewModel> GetDietPlanByIdAsync(int id);
        Task<List<DietPlanViewModel>> GetDietPlansByPatientIdAsync(int patientId);
        Task<List<DietPlanViewModel>> GetDietPlansByDiyetisyenIdAsync(int diyetisyenId); // Eksik olan yöntem
    }
}
