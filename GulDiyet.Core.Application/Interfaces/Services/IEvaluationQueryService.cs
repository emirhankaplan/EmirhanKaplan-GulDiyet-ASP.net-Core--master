using System.Collections.Generic;
using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.Evaluation;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IEvaluationQueryService
    {
        Task<List<SaveEvaluationViewModel>> GetAllViewModel();
        Task<SaveEvaluationViewModel> GetByIdSaveViewModel(int id);
        Task<List<EvaluationViewModel>> GetEvaluationsByDoctorId(int doctorId);
        Task<List<EvaluationViewModel>> GetEvaluationsByAppointmentId(int appointmentId);
    }
}
