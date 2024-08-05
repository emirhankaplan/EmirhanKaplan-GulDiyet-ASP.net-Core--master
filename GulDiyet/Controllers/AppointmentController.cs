using GulDiyet.Core.Application.Enums;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Appointment;
using GulDiyet.Core.Application.ViewModels.LaboratoryResult;
using GulDiyet.Core.Application.Helpers;
using GulDiyet.Middlewares;
using Microsoft.AspNetCore.Mvc;
using GulDiyet.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using GulDiyet.Core.Application.ViewModels.Diyetisyens;
using GulDiyet.Core.Application.ViewModels.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using GulDiyet.Core.Application.ViewModels.Evaluation;
using GulDiyet.Core.Application.Services;

namespace GulDiyet.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;
        private readonly IDiyetisyenService _DiyetisyenService;
        private readonly ILaboratoryResultService _labResultService;
        private readonly ILaboratoryTestService _labTestService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel? userViewModel;
        private readonly IEvaluationService _evaluationService;
        private readonly IPdfReportService _pdfReportService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IPatientService patientService,
            IDiyetisyenService DiyetisyenService,
            ILaboratoryResultService labResultService,
            ILaboratoryTestService labTestService,
            ValidateUserSession validateUserSession,
            IHttpContextAccessor httpContextAccessor,
            IEvaluationService evaluationService,
            IPdfReportService pdfReportService)
        {
            _evaluationService = evaluationService;
            _pdfReportService = pdfReportService;
            _appointmentService = appointmentService;
            _patientService = patientService;
            _DiyetisyenService = DiyetisyenService;
            _labResultService = labResultService;
            _labTestService = labTestService;
            _validateUserSession = validateUserSession;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<IActionResult> CompleteAppointment(int appointmentId)
        {
            var appointment = await _appointmentService.GetByIdSaveViewModel(appointmentId);
            if (appointment != null)
            {
                appointment.Status = Status.Completed;
                await _appointmentService.Update(appointment);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DownloadPdf(int appointmentId)
        {
            var appointment = await _appointmentService.GetById(appointmentId);
            var pdf = await _pdfReportService.GeneratePdf(appointment);
            return File(pdf, "application/pdf", "AppointmentDetails.pdf");
        }

        public async Task<IActionResult> AddEvaluation(SaveEvaluationViewModel vm)
        {
            await _evaluationService.AddEvaluationAsync(vm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var list = userViewModel.TypeUserId == Roles.Diyetisyen
                ? await _appointmentService.GetAppointmentsByDiyetisyenId(userViewModel.Id)
                : await _appointmentService.GetAllViewModel();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var listPatients = await _patientService.GetAllViewModel();
            var listDiyetisyen = await _DiyetisyenService.GetAllViewModel();

            if (listPatients == null)
            {
                listPatients = new List<PatientViewModel>();
            }

            if (listDiyetisyen == null)
            {
                listDiyetisyen = new List<DiyetisyenViewModel>();
            }

            var patientsList = listPatients.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.FirstName }).ToList();
            var diyetisyenList = listDiyetisyen.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.FirstName }).ToList();

            ViewBag.Patients = patientsList;
            ViewBag.Diyetisyen = diyetisyenList;

            return View(new SaveAppointmentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveAppointmentViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                var listPatients = await _patientService.GetAllViewModel();
                ViewBag.Patients = listPatients ?? new List<PatientViewModel>();

                var listDiyetisyen = await _DiyetisyenService.GetAllViewModel();
                ViewBag.Diyetisyen = listDiyetisyen ?? new List<DiyetisyenViewModel>();

                return View(vm);
            }

            await _appointmentService.Add(vm);
            return RedirectToRoute(new { controller = "Appointment", action = "Index" });
        }

        public async Task<IActionResult> Consult(int appointmentId)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            SaveLaboratoryResultViewModel model = new()
            {
                AppointmentId = appointmentId,
                LaboratoryTests = await _labTestService.GetAllViewModel()
            };
            return View("ConsultPatient", model);
        }

        [HttpPost]
        public async Task<IActionResult> Consult(SaveLaboratoryResultViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Assistant)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                vm.LaboratoryTests = await _labTestService.GetAllViewModel();
                return View("ConsultPatient", vm);
            }

            await _labResultService.Add(vm);

            var appointment = await _appointmentService.GetByIdSaveViewModel(vm.AppointmentId);
            appointment.Status = Status.PendingResults;

            await _appointmentService.Update(appointment);

            return RedirectToRoute(new { controller = "Appointment", action = "Index" });
        }

        public async Task<IActionResult> CheckResults(FilterLabResultViewModel filterLabResult, string? status)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var list = await _labResultService.GetAllViewModelWithFilter(filterLabResult);

            if (status != null)
            {
                ViewBag.Status = status;
            }
            return View("LaboratoryResults", list);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var appointment = await _appointmentService.GetByIdSaveViewModel(id);
            return View(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            await _appointmentService.Delete(id);
            return RedirectToRoute(new { controller = "Appointment", action = "Index" });
        }

        [HttpGet]
        public async Task<IActionResult> AvailablePeriods(int diyetisyenId, DateTime day)
        {
            var availablePeriods = await _appointmentService.GetAvailablePeriods(diyetisyenId, day);
            return Json(availablePeriods);
        }
    }
}
