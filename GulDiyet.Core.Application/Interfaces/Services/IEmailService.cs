using System.Collections.Generic;
using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.Email;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task<SaveEmailViewModel> Add(SaveEmailViewModel vm);
        Task Update(SaveEmailViewModel vm);
        Task Delete(int id);
        Task<List<EmailViewModel>> GetAllViewModel();
        Task<SaveEmailViewModel> GetByIdSaveViewModel(int id);
        Task SendEmailAsync(SaveEmailViewModel emailViewModel);
        void NotifyNewDietPlan(SaveEmailViewModel emailViewModel);
        Task SendEmailToAllUsers(EmailViewModel emailViewModel); // Eksik olan yöntem
        Task SendBulkEmails(List<SaveEmailViewModel> emails); // Eksik olan yöntem
    }
}
