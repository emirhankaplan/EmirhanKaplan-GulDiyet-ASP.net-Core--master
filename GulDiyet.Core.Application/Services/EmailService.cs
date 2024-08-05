using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Email;
using GulDiyet.Core.Domain.Entities;
using System.Collections.Generic;

namespace GulDiyet.Core.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailRepository _emailRepository;
        private readonly SmtpConfig _smtpConfig;
        private readonly IRabbitMQService _rabbitMQService;

        public EmailService(IEmailRepository emailRepository, IOptions<SmtpConfig> smtpConfig, IRabbitMQService rabbitMQService)
        {
            _emailRepository = emailRepository;
            _smtpConfig = smtpConfig.Value;
            _rabbitMQService = rabbitMQService;
        }

        public async Task<SaveEmailViewModel> Add(SaveEmailViewModel vm)
        {
            Email email = new()
            {
                To = vm.To,
                Subject = vm.Subject,
                Body = vm.Body,
                SentDate = DateTime.UtcNow
            };

            email = await _emailRepository.AddAsync(email);
            vm.Id = email.Id;

            await SendEmailAsync(vm);

            return vm;
        }

        public async Task Update(SaveEmailViewModel vm)
        {
            Email email = await _emailRepository.GetByIdAsync(vm.Id);
            email.To = vm.To;
            email.Subject = vm.Subject;
            email.Body = vm.Body;

            await _emailRepository.UpdateAsync(email);
        }

        public async Task Delete(int id)
        {
            var email = await _emailRepository.GetByIdAsync(id);
            if (email != null)
            {
                await _emailRepository.DeleteAsync(email);
            }
        }

        public async Task<List<EmailViewModel>> GetAllViewModel()
        {
            var emails = await _emailRepository.GetAllAsync();
            return emails.Select(e => new EmailViewModel
            {
                Id = e.Id,
                To = e.To,
                Subject = e.Subject,
                Body = e.Body,
                SentDate = e.SentDate
            }).ToList();
        }

        public async Task<SaveEmailViewModel> GetByIdSaveViewModel(int id)
        {
            var email = await _emailRepository.GetByIdAsync(id);
            return new SaveEmailViewModel
            {
                Id = email.Id,
                To = email.To,
                Subject = email.Subject,
                Body = email.Body
            };
        }

        public async Task SendEmailAsync(SaveEmailViewModel emailViewModel)
        {
            var client = new SmtpClient(_smtpConfig.Host, _smtpConfig.Port)
            {
                Credentials = new NetworkCredential(_smtpConfig.User, _smtpConfig.Password),
                EnableSsl = _smtpConfig.UseSSL
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpConfig.User),
                Subject = emailViewModel.Subject,
                Body = emailViewModel.Body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(emailViewModel.To);

            try
            {
                await client.SendMailAsync(mailMessage);
                Console.WriteLine($"Email sent to: {emailViewModel.To}, Subject: {emailViewModel.Subject}, Body: {emailViewModel.Body}");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
        }

        public void NotifyNewDietPlan(SaveEmailViewModel emailViewModel)
        {
            _rabbitMQService.SendMessage(emailViewModel);
        }

        public async Task SendBulkEmailAsync(List<SaveEmailViewModel> emailViewModels)
        {
            foreach (var emailViewModel in emailViewModels)
            {
                await SendEmailAsync(emailViewModel);
            }
        }

        public async Task SendEmailToAllUsers(EmailViewModel emailViewModel)
        {
            var emails = await _emailRepository.GetAllAsync();
            var emailViewModels = emails.Select(e => new SaveEmailViewModel
            {
                To = e.To,
                Subject = emailViewModel.Subject,
                Body = emailViewModel.Body
            }).ToList();

            await SendBulkEmailAsync(emailViewModels);
        }
        public async Task SendBulkEmails(List<SaveEmailViewModel> emails)
        {
            foreach (var email in emails)
            {
                await SendEmailAsync(email);
            }
        }
        public async Task SendBulkEmails(List<EmailViewModel> emails)
        {
            var emailViewModels = emails.Select(e => new SaveEmailViewModel
            {
                To = e.To,
                Subject = e.Subject,
                Body = e.Body
            }).ToList();

            await SendBulkEmailAsync(emailViewModels);
        }
    }
}
