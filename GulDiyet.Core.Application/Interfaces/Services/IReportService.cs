using GulDiyet.Core.Application.ViewModels.Report;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IReportService
    {
        Task<DoctorPerformanceReportViewModel> GetDoctorPerformanceReportAsync(int diyetisyenId);
    }
}
