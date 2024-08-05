using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Evaluation;
using Microsoft.AspNetCore.SignalR;
using GulDiyet.Core.Application.Hubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EvaluationService(IEvaluationRepository evaluationRepository, IHubContext<NotificationHub> hubContext)
        {
            _evaluationRepository = evaluationRepository;
            _hubContext = hubContext;
        }

        public async Task Add(SaveEvaluationViewModel vm)
        {
            var evaluation = new Evaluation
            {
                AppointmentId = vm.AppointmentId,
                Rating = vm.Rating,
                Feedback = vm.Feedback
            };
            await _evaluationRepository.AddAsync(evaluation);

            await _hubContext.Clients.All.SendAsync("ReceiveEvaluationUpdate", evaluation.Id);
        }

        public async Task AddEvaluationAsync(SaveEvaluationViewModel vm) // Bu metodu ekleyin
        {
            var evaluation = new Evaluation
            {
                AppointmentId = vm.AppointmentId,
                Rating = vm.Rating,
                Feedback = vm.Feedback
            };
            await _evaluationRepository.AddAsync(evaluation);

            await _hubContext.Clients.All.SendAsync("ReceiveEvaluationUpdate", evaluation.Id);
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

        public async Task Update(SaveEvaluationViewModel vm)
        {
            var evaluation = await _evaluationRepository.GetByIdAsync(vm.Id);
            if (evaluation != null)
            {
                evaluation.AppointmentId = vm.AppointmentId;
                evaluation.Rating = vm.Rating;
                evaluation.Feedback = vm.Feedback;
                await _evaluationRepository.UpdateAsync(evaluation);

                await _hubContext.Clients.All.SendAsync("ReceiveEvaluationUpdate", evaluation.Id);
            }
        }

        public async Task Delete(int id)
        {
            var evaluation = await _evaluationRepository.GetByIdAsync(id);
            if (evaluation != null)
            {
                await _evaluationRepository.DeleteAsync(evaluation);

                await _hubContext.Clients.All.SendAsync("ReceiveEvaluationUpdate", id);
            }
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
