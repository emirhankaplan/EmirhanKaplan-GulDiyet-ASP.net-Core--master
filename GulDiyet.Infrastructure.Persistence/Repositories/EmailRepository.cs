using System.Net;
using System.Net.Mail;
using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.ViewModels.Email;
using GulDiyet.Core.Domain.Entities;
using GulDiyet.Infrastructure.Persistence.Contexts;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GulDiyet.Infrastructure.Persistence.Repositories
{
    public class EmailRepository : GenericRepository<Email>, IEmailRepository
    {
        private readonly SmtpConfig _smtpConfig;

        public EmailRepository(ApplicationDbContext dbContext, IOptions<SmtpConfig> smtpConfig) : base(dbContext)
        {
            _smtpConfig = smtpConfig.Value;
        }

        public async Task SendEmailAsync(SaveEmailViewModel emailViewModel)
        {
            try
            {
                using var client = new SmtpClient(_smtpConfig.Host, _smtpConfig.Port)
                {
                    Credentials = new NetworkCredential(_smtpConfig.User, _smtpConfig.Password),
                    EnableSsl = _smtpConfig.UseSSL,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpConfig.User),
                    Subject = emailViewModel.Subject,
                    Body = emailViewModel.Body,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(emailViewModel.To);

                await client.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SendBulkEmailAsync(List<SaveEmailViewModel> emailViewModels)
        {
            foreach (var emailViewModel in emailViewModels)
            {
                await SendEmailAsync(emailViewModel);
            }
        }
    }
}
