using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Evaluation;
using GulDiyet.Middlewares;
using GulDiyet.Core.Application.Enums;
using GulDiyet.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using GulDiyet.Core.Application.Helpers;

namespace GulDiyet.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IEvaluationService _evaluationService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel? _userViewModel;

        public FeedbackController(IEvaluationService evaluationService, ValidateUserSession validateUserSession, IHttpContextAccessor httpContextAccessor)
        {
            _evaluationService = evaluationService;
            _validateUserSession = validateUserSession;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser() || _userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var list = await _evaluationService.GetAllViewModel();
            return View(list);
        }

        public IActionResult Create(int appointmentId)
        {
            if (!_validateUserSession.HasUser() || _userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return View(new SaveEvaluationViewModel { AppointmentId = appointmentId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveEvaluationViewModel vm)
        {
            if (!_validateUserSession.HasUser() || _userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _evaluationService.Add(vm);
            return RedirectToRoute(new { controller = "Feedback", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser() || _userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var evaluation = await _evaluationService.GetByIdSaveViewModel(id);
            return View(evaluation);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveEvaluationViewModel vm)
        {
            if (!_validateUserSession.HasUser() || _userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _evaluationService.Update(vm);
            return RedirectToRoute(new { controller = "Feedback", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser() || _userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var evaluation = await _evaluationService.GetByIdSaveViewModel(id);
            return View(evaluation);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_validateUserSession.HasUser() || _userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            await _evaluationService.Delete(id);
            return RedirectToRoute(new { controller = "Feedback", action = "Index" });
        }

        public async Task<IActionResult> GeneratePdf(int id)
        {
            var evaluation = await _evaluationService.GetByIdSaveViewModel(id);
            var pdfBytes = await PdfHelper.CreateEvaluationPdf(evaluation);

            return File(pdfBytes, "application/pdf", $"Evaluation_{id}.pdf");
        }

        // Yeni eklenen metodlar
        public async Task<IActionResult> ViewDoctorEvaluations(int doctorId)
        {
            if (!_validateUserSession.HasUser() || _userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var evaluations = await _evaluationService.GetEvaluationsByDoctorId(doctorId);
            return View("DoctorEvaluations", evaluations);
        }

        public async Task<IActionResult> ViewAppointmentEvaluations(int appointmentId)
        {
            if (!_validateUserSession.HasUser() || _userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var evaluations = await _evaluationService.GetEvaluationsByAppointmentId(appointmentId);
            return View("AppointmentEvaluations", evaluations);
        }
    }
}
