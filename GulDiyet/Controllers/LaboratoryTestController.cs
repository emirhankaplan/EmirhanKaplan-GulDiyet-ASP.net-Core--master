using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.LaboratoryTests;
using GulDiyet.Core.Application.ViewModels.Users;
using GulDiyet.Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using GulDiyet.Middlewares;
using GulDiyet.Core.Application.Enums;

namespace GulDiyet.Controllers
{
    public class LaboratoryTestController : Controller
    {
        private readonly ILaboratoryTestService _labTestService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel userViewModel;

        public LaboratoryTestController(ILaboratoryTestService labTestService, ValidateUserSession validateUserSession,
            IHttpContextAccessor httpContextAccessor)
        {
            _labTestService = labTestService;
            _validateUserSession = validateUserSession;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            var list = await _labTestService.GetAllViewModel();
            return View(list);
        }

        public IActionResult Create()
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            SaveLaboratoryTestViewModel vm = new();
            return View("SaveLaboratoryTest", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveLaboratoryTestViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View("SaveLaboratoryTest", vm);
            }

            await _labTestService.Add(vm);
            return RedirectToRoute(new { controller = "LaboratoryTest", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var labTest = await _labTestService.GetByIdSaveViewModel(id);
            return View("SaveLaboratoryTest", labTest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveLaboratoryTestViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View("SaveLaboratoryTest", vm);
            }

            await _labTestService.Update(vm);
            return RedirectToRoute(new { controller = "LaboratoryTest", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var labTest = await _labTestService.GetByIdSaveViewModel(id);
            return View(labTest);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLabTest(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            await _labTestService.Delete(id);
            return RedirectToRoute(new { controller = "LaboratoryTest", action = "Index" });
        }

        // Yeni eklenen metotlar
        public async Task<IActionResult> ViewTestResults(int testId)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var results = await _labTestService.GetTestResults(testId);
            return View("TestResults", results);
        }

      
    }
}
