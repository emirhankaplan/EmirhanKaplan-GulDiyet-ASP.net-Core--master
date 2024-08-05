using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.LaboratoryResult;
using GulDiyet.Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using GulDiyet.Core.Application.ViewModels.Users;
using GulDiyet.Middlewares;
using GulDiyet.Core.Application.Enums;

namespace GulDiyet.Controllers
{
    public class LaboratoryResultController : Controller
    {
        private readonly ILaboratoryResultService _labResultService;
        private readonly ILaboratoryTestService _labTestService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel? userViewModel;

        public LaboratoryResultController(ILaboratoryResultService labResultService, ILaboratoryTestService labTestService,
            ValidateUserSession validateUserSession, IHttpContextAccessor httpContextAccessor)
        {
            _labResultService = labResultService;
            _labTestService = labTestService;
            _validateUserSession = validateUserSession;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<IActionResult> Index(FilterLabResultViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Diyetisyen)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            var list = await _labResultService.GetAllViewModelWithFilter(vm);
            return View(list);
        }

        public async Task<IActionResult> ReportResult(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Diyetisyen)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            var labResult = await _labResultService.GetByIdSaveViewModel(id);
            return View("ReportResult", labResult);
        }

        [HttpPost]
        public async Task<IActionResult> ReportResult(SaveLaboratoryResultViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Diyetisyen)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            if (!ModelState.IsValid)
            {
                return View("ReportResult", vm);
            }

            vm.IsCompleted = true;

            await _labResultService.Update(vm);
            return RedirectToRoute(new { controller = "LaboratoryResult", action = "Index" });
        }

        // Yeni eklenen metotlar
        public async Task<IActionResult> ViewResultsByAppointmentId(int appointmentId)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Diyetisyen)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var results = await _labResultService.GetByAppointmentId(appointmentId);
            return View("ResultsByAppointment", results);
        }

       
    }
}
