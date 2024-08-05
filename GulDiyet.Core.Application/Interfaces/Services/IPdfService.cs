using System.Collections.Generic;
using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.DietPlan;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IPdfService
    {
        Task<byte[]> GenerateDietPlanPdf(List<DietPlanViewModel> dietPlans);
    }
}
