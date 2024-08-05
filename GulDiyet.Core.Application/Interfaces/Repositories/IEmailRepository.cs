using GulDiyet.Core.Application.ViewModels.Email;
using GulDiyet.Core.Domain.Entities;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.Interfaces.Repositories
{
    public interface IEmailRepository : IGenericRepository<Email>
    {
        Task SendEmailAsync(SaveEmailViewModel emailViewModel);
        Task SendBulkEmailAsync(List<SaveEmailViewModel> emailViewModels);
    }
}
