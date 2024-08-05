using System.Threading.Tasks;
using GulDiyet.Core.Application.Enums;
using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Users;
using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<SaveUserViewModel> Add(SaveUserViewModel vm)
        {
            User user = new();
            user.Name = vm.Name;
            user.LastName = vm.LastName;
            user.Email = vm.Email;
            user.Phone = vm.Phone;
            user.Username = vm.Username;
            user.Password = vm.Password;
            user.TypeUserId = (int)vm.TypeUserId;

            user = await _userRepository.AddAsync(user);

            SaveUserViewModel userVm = new();
            userVm.Id = user.Id;
            userVm.Name = user.Name;
            userVm.LastName = user.LastName;
            userVm.Email = user.Email;
            userVm.Phone = user.Phone;
            userVm.Username = user.Username;
            userVm.Password = user.Password;
            userVm.TypeUserId = (Roles)user.TypeUserId;

            return userVm;
        }

        public async Task<UserViewModel> Login(LoginViewModel loginVm)
        {
            User user = await _userRepository.LoginAsync(loginVm);

            if (user == null)
            {
                return null;
            }

            UserViewModel userVm = new();
            userVm.Id = user.Id;
            userVm.Name = user.Name;
            userVm.LastName = user.LastName;
            userVm.Email = user.Email;
            userVm.Phone = user.Phone;
            userVm.Username = user.Username;
            userVm.Password = user.Password;
            userVm.TypeUserId = (Roles)user.TypeUserId;

            return userVm;
        }

        public async Task Update(SaveUserViewModel vm)
        {
            User user = await _userRepository.GetByIdAsync(vm.Id);

            user.Id = vm.Id;
            user.Name = vm.Name;
            user.Email = vm.Email;
            user.Phone = vm.Phone;
            user.Username = vm.Username;
            user.LastName = vm.LastName;
            user.Password = vm.Password;
            user.TypeUserId = (int)vm.TypeUserId;

            await _userRepository.UpdateAsync(user);
        }

        public async Task Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user != null)
            {
                await _userRepository.DeleteAsync(user);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<List<UserViewModel>> GetAllViewModel()
        {
            var list = await _userRepository.GetAllAsync();
            return list.Select(u => new UserViewModel
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                Username = u.Username,
                LastName = u.LastName,
                Password = u.Password,
                TypeUserId = (Roles)u.TypeUserId
            }).ToList();
        }

        public async Task<SaveUserViewModel> GetByIdSaveViewModel(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            SaveUserViewModel vm = new();
            vm.Id = user.Id;
            vm.Name = user.Name;
            vm.LastName = user.LastName;
            vm.Email = user.Email;
            vm.Phone = user.Phone;
            vm.Username = user.Username;
            vm.Password = user.Password;
            vm.TypeUserId = (Roles)user.TypeUserId;

            return vm;
        }

        public async Task UpdatePassword(SaveUserViewModel vm)
        {
            User user = await _userRepository.GetByIdAsync(vm.Id);
            user.Password = vm.Password;
            await _userRepository.UpdateAsync(user);
        }
    }
}
