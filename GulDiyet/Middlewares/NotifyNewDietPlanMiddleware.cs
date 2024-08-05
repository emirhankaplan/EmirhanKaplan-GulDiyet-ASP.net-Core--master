using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Email;

namespace GulDiyet.Middlewares
{
    public class NotifyNewDietPlanMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEmailService _emailService;

        public NotifyNewDietPlanMiddleware(RequestDelegate next, IEmailService emailService)
        {
            _next = next;
            _emailService = emailService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Diyet planı bildirimini yap
            var emailViewModel = new SaveEmailViewModel
            {
                To = "eraykelesk@gmail.com",
                Subject = "Yeni Diyet Planı",
                Body = "Yeni bir diyet planı oluşturuldu."
            };
            _emailService.NotifyNewDietPlan(emailViewModel);

            await _next(context);
        }
    }
}
