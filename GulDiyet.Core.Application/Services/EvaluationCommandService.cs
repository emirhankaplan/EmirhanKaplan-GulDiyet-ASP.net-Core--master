using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Evaluation;
using Microsoft.AspNetCore.SignalR;
using GulDiyet.Core.Application.Hubs;
using System.Threading.Tasks;
using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.Services
{
    public class EvaluationCommandService : IEvaluationCommandService
    {
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EvaluationCommandService(IEvaluationRepository evaluationRepository, IHubContext<NotificationHub> hubContext)
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
    }
}
