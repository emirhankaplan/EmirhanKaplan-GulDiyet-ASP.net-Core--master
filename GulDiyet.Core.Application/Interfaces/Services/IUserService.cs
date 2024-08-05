using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.Users;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<SaveUserViewModel, UserViewModel>
    {
        Task<UserViewModel> Login(LoginViewModel loginVm);
        Task UpdatePassword(SaveUserViewModel vm); // Eksik olan yöntem
    }
}
