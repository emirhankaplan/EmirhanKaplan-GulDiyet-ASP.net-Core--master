using System.Collections.Generic;
using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.Evaluation;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IEvaluationService
    {
        Task<List<SaveEvaluationViewModel>> GetAllViewModel();
        Task Add(SaveEvaluationViewModel vm);
        Task Update(SaveEvaluationViewModel vm);
        Task Delete(int id);
        Task<SaveEvaluationViewModel> GetByIdSaveViewModel(int id);
        Task AddEvaluationAsync(SaveEvaluationViewModel saveEvaluationViewModel);
        Task<List<EvaluationViewModel>> GetEvaluationsByDoctorId(int doctorId); // Eksik olan yöntem
        Task<List<EvaluationViewModel>> GetEvaluationsByAppointmentId(int appointmentId); // Eksik olan yöntem
    }
}
