using GulDiyet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using GulDiyet.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using GulDiyet.Core.Application.ViewModels.Users;
using GulDiyet.Middlewares;
using GulDiyet.Core.Application.Enums;
using GulDiyet.Core.Application.Helpers;
using GulDiyet.Core.Application.ViewModels.Email;

namespace GulDiyet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ValidateUserSession _validateUserSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly UserViewModel? _userViewModel;

        public HomeController(ILogger<HomeController> logger, ValidateUserSession validateUserSession, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            _logger = logger;
            _validateUserSession = validateUserSession;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
        }

        public IActionResult Index()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            return View();
        }

        public IActionResult Privacy()
        {
            if (!_validateUserSession.HasUser())
            {
                return RedirectToRoute(new { controller = "User", action = "Login" });
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Yeni eklenen metodlar
        public async Task<IActionResult> SendBulkEmails()
        {
            if (!_validateUserSession.HasUser() || _userViewModel.TypeUserId != Roles.Admin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var emails = await _emailService.GetAllViewModel(); // Tüm e-posta kayýtlarýný getir
            await _emailService.SendBulkEmails(emails.Select(e => new SaveEmailViewModel
            {
                To = e.To,
                Subject = e.Subject,
                Body = e.Body
            }).ToList());

            return RedirectToAction("Index");
        }

    }
}
