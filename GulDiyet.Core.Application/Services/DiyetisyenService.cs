using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Diyetisyens;
using GulDiyet.Core.Application.ViewModels.Appointment;
using System.Collections.Generic;
using System.Threading.Tasks;
using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.Services
{
    public class DiyetisyenService : IDiyetisyenService
    {
        private readonly IDiyetisyenRepository _diyetisyenRepository;
        private readonly IAppointmentService _appointmentService;

        public DiyetisyenService(IDiyetisyenRepository diyetisyenRepository, IAppointmentService appointmentService)
        {
            _diyetisyenRepository = diyetisyenRepository;
            _appointmentService = appointmentService;
        }

        public async Task<SaveDiyetisyenViewModel> Add(SaveDiyetisyenViewModel vm)
        {
            Diyetisyen diyetisyen = new();
            diyetisyen.FirstName = vm.FirstName;
            diyetisyen.LastName = vm.LastName;
            diyetisyen.Email = vm.Email;
            diyetisyen.Phone = vm.Phone;
            diyetisyen.IdNumber = vm.IdNumber;
            diyetisyen.ImagePath = vm.ImageUrl;

            diyetisyen = await _diyetisyenRepository.AddAsync(diyetisyen);

            SaveDiyetisyenViewModel diyetisyenVm = new();
            diyetisyenVm.Id = diyetisyen.Id;
            diyetisyenVm.FirstName = diyetisyen.FirstName;
            diyetisyenVm.LastName = diyetisyen.LastName;
            diyetisyenVm.Email = diyetisyen.Email;
            diyetisyenVm.Phone = diyetisyen.Phone;
            diyetisyenVm.IdNumber = diyetisyen.IdNumber;
            diyetisyenVm.ImageUrl = diyetisyen.ImagePath;

            return diyetisyenVm;
        }

        public async Task Update(SaveDiyetisyenViewModel vm)
        {
            Diyetisyen diyetisyen = await _diyetisyenRepository.GetByIdAsync(vm.Id);

            diyetisyen.Id = vm.Id;
            diyetisyen.FirstName = vm.FirstName;
            diyetisyen.LastName = vm.LastName;
            diyetisyen.Email = vm.Email;
            diyetisyen.Phone = vm.Phone;
            diyetisyen.IdNumber = vm.IdNumber;
            diyetisyen.ImagePath = vm.ImageUrl;

            await _diyetisyenRepository.UpdateAsync(diyetisyen);
        }

        public async Task Delete(int id)
        {
            var diyetisyen = await _diyetisyenRepository.GetByIdAsync(id);

            if (diyetisyen != null)
            {
                await _diyetisyenRepository.DeleteAsync(diyetisyen);
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task<List<DiyetisyenViewModel>> GetAllViewModel()
        {
            var list = await _diyetisyenRepository.GetAllAsync();
            return list.Select(d => new DiyetisyenViewModel
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
                Phone = d.Phone,
                IdNumber = d.IdNumber,
                ImageUrl = d.ImagePath
            }).ToList();
        }

        public async Task<SaveDiyetisyenViewModel> GetByIdSaveViewModel(int id)
        {
            var diyetisyen = await _diyetisyenRepository.GetByIdAsync(id);

            SaveDiyetisyenViewModel vm = new();
            vm.Id = diyetisyen.Id;
            vm.FirstName = diyetisyen.FirstName;
            vm.LastName = diyetisyen.LastName;
            vm.Email = diyetisyen.Email;
            vm.Phone = diyetisyen.Phone;
            vm.IdNumber = diyetisyen.IdNumber;
            vm.ImageUrl = diyetisyen.ImagePath;

            return vm;
        }

        public async Task<List<AppointmentViewModel>> GetAppointmentsByDiyetisyenId(int diyetisyenId) // Bu metodu ekleyin
        {
            return await _appointmentService.GetAppointmentsByDiyetisyenId(diyetisyenId);
        }
    }
}
