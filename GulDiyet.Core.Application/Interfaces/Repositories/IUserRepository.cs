using GulDiyet.Core.Application.ViewModels.Users;
using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> LoginAsync(LoginViewModel loginVm);
    }
}
