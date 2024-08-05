using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.DietPlan;
using Microsoft.AspNetCore.Mvc;
using GulDiyet.Middlewares;
using GulDiyet.Core.Application.Enums;
using GulDiyet.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using GulDiyet.Core.Application.Helpers;
using System.Threading.Tasks;

namespace GulDiyet.Controllers
{
    public class DietPlanController : Controller
    {
        private readonly IDietPlanService _dietPlanService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel? userViewModel;

        public DietPlanController(IDietPlanService dietPlanService, ValidateUserSession validateUserSession, IHttpContextAccessor httpContextAccessor)
        {
            _dietPlanService = dietPlanService;
            _validateUserSession = validateUserSession;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public async Task<IActionResult> Index()
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Diyetisyen)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var list = await _dietPlanService.GetDietPlansByDiyetisyenIdAsync(userViewModel.Id);
            return View(list);
        }

        public IActionResult Create(int patientId)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Diyetisyen)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return View(new SaveDietPlanViewModel { PatientId = patientId, DiyetisyenId = userViewModel.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveDietPlanViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Diyetisyen)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _dietPlanService.CreateDietPlanAsync(vm);
            return RedirectToRoute(new { controller = "DietPlan", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Diyetisyen)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var dietPlan = await _dietPlanService.GetDietPlanByIdAsync(id);
            return View(dietPlan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveDietPlanViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Diyetisyen)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _dietPlanService.UpdateDietPlanAsync(vm);
            return RedirectToRoute(new { controller = "DietPlan", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Diyetisyen)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var dietPlan = await _dietPlanService.GetDietPlanByIdAsync(id);
            return View(dietPlan);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Diyetisyen)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            await _dietPlanService.DeleteDietPlanAsync(id);
            return RedirectToRoute(new { controller = "DietPlan", action = "Index" });
        }
    }
}
