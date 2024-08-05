using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Report;
using System.Linq;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Services
{
    //facede Structural ıkı servısle uyumlu ıs yapıyor doktorun performans raporunu olusturmak ıcın
    public class ReportService : IReportService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IEvaluationRepository _evaluationRepository;

        public ReportService(IAppointmentRepository appointmentRepository, IEvaluationRepository evaluationRepository)
        {
            _appointmentRepository = appointmentRepository;
            _evaluationRepository = evaluationRepository;
        }

        public async Task<DoctorPerformanceReportViewModel> GetDoctorPerformanceReportAsync(int doctorId)
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            var doctorAppointments = appointments.Where(a => a.DiyetisyenId == doctorId).ToList();

            var evaluations = await _evaluationRepository.GetAllAsync();
            var doctorEvaluations = evaluations.Where(e => doctorAppointments.Any(a => a.Id == e.AppointmentId)).ToList();

            var totalRatings = doctorEvaluations.Sum(e => e.Rating);
            var averageRating = (doctorEvaluations.Count == 0) ? 0 : (double)totalRatings / doctorEvaluations.Count;

            return new DoctorPerformanceReportViewModel
            {
                DoctorId = doctorId,
                TotalAppointments = doctorAppointments.Count,
                AverageRating = averageRating,
                Feedbacks = doctorEvaluations.Select(e => e.Feedback).ToList()
            };
        }
    }
}
