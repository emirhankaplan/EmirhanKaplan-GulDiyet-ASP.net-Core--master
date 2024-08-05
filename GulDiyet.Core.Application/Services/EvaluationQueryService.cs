using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Evaluation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Services
{
    public class EvaluationQueryService : IEvaluationQueryService
    {
        private readonly IEvaluationRepository _evaluationRepository;

        public EvaluationQueryService(IEvaluationRepository evaluationRepository)
        {
            _evaluationRepository = evaluationRepository;
        }

        public async Task<List<SaveEvaluationViewModel>> GetAllViewModel()
        {
            var evaluations = await _evaluationRepository.GetAllAsync();
            return evaluations.Select(e => new SaveEvaluationViewModel
            {
                Id = e.Id,
                AppointmentId = e.AppointmentId,
                Rating = e.Rating,
                Feedback = e.Feedback
            }).ToList();
        }

        public async Task<SaveEvaluationViewModel> GetByIdSaveViewModel(int id)
        {
            var evaluation = await _evaluationRepository.GetByIdAsync(id);
            if (evaluation == null)
                return null;

            return new SaveEvaluationViewModel
            {
                Id = evaluation.Id,
                AppointmentId = evaluation.AppointmentId,
                Rating = evaluation.Rating,
                Feedback = evaluation.Feedback
            };
        }

        public async Task<List<EvaluationViewModel>> GetEvaluationsByDoctorId(int doctorId)
        {
            var evaluations = await _evaluationRepository.GetEvaluationsByDoctorIdAsync(doctorId);
            return evaluations.Select(e => new EvaluationViewModel
            {
                Id = e.Id,
                AppointmentId = e.AppointmentId,
                Rating = e.Rating,
                Feedback = e.Feedback
            }).ToList();
        }

        public async Task<List<EvaluationViewModel>> GetEvaluationsByAppointmentId(int appointmentId)
        {
            var evaluations = await _evaluationRepository.GetEvaluationsByAppointmentIdAsync(appointmentId);
            return evaluations.Select(e => new EvaluationViewModel
            {
                Id = e.Id,
                AppointmentId = e.AppointmentId,
                Rating = e.Rating,
                Feedback = e.Feedback
            }).ToList();
        }
    }
}
