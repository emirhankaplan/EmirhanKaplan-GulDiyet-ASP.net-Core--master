using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Email;
using Microsoft.AspNetCore.Mvc;
using GulDiyet.Middlewares;
using GulDiyet.Core.Application.Enums;
using GulDiyet.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using GulDiyet.Core.Application.Helpers;
using System.Threading.Tasks;

namespace GulDiyet.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserViewModel? userViewModel;

        public EmailController(IEmailService emailService, ValidateUserSession validateUserSession, IHttpContextAccessor httpContextAccessor)
        {
            _emailService = emailService;
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

            var list = await _emailService.GetAllViewModel();
            return View(list);
        }

        public IActionResult Create()
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            return View(new SaveEmailViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveEmailViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _emailService.Add(vm);
            return RedirectToRoute(new { controller = "Email", action = "Index" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var email = await _emailService.GetByIdSaveViewModel(id);
            return View(email);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveEmailViewModel vm)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            await _emailService.Update(vm);
            return RedirectToRoute(new { controller = "Email", action = "Index" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var email = await _emailService.GetByIdSaveViewModel(id);
            return View(email);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            await _emailService.Delete(id);
            return RedirectToRoute(new { controller = "Email", action = "Index" });
        }

        public async Task<IActionResult> SendEmailToAllUsers(string subject, string body)
        {
            if (!_validateUserSession.HasUser() || userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var allEmails = await _emailService.GetAllViewModel();
            var emailViewModels = allEmails.Select(e => new SaveEmailViewModel
            {
                To = e.To,
                Subject = subject,
                Body = body
            }).ToList();

            await _emailService.SendBulkEmails(emailViewModels);
            return RedirectToRoute(new { controller = "Email", action = "Index" });
        }

    }
}
