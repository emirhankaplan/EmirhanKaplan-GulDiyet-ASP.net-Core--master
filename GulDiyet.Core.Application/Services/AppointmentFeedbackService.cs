using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Appointment;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Services
{
    public class AppointmentFeedbackService : IAppointmentFeedbackService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentFeedbackService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task SaveFeedbackAsync(AppointmentFeedbackViewModel feedbackVm)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(feedbackVm.AppointmentId);
            if (appointment != null)
            {
                appointment.Rating = feedbackVm.Rating;
                appointment.Feedback = feedbackVm.Feedback;
                await _appointmentRepository.UpdateAsync(appointment);
            }
        }
    }
}
