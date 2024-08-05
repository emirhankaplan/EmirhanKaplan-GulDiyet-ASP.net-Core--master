using GulDiyet.Core.Application.Enums;
using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.LaboratoryResult;
using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.Services
{
    public class LaboratoryResultService : ILaboratoryResultService
    {
        private readonly ILaboratoryResultRepository _labResultRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public LaboratoryResultService(ILaboratoryResultRepository labResultRepository, IAppointmentRepository appointmentRepository)
        {
            _labResultRepository = labResultRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<LaboratoryResultViewModel>> GetResultsByAppointmentId(int appointmentId)
        {
            var list = await _labResultRepository.GetAllWithIncludeAsync(new List<string> { "Appointment", "LaboratoryTest", "Appointment.Patient" });

            return list
                .Where(labVm => labVm.AppointmentId == appointmentId)
                .Select(labVm => new LaboratoryResultViewModel
                {
                    Id = labVm.Id,
                    AppointmentId = labVm.AppointmentId,
                    PatientName = $"{labVm.Appointment.Patient.FirstName} {labVm.Appointment.Patient.LastName}",
                    PatientIdNumber = labVm.Appointment.Patient.IdNumber,
                    LaboratoryTestId = labVm.LaboratoryTestId,
                    LaboratoryTestName = labVm.LaboratoryTest.Name,
                    IsCompleted = labVm.IsCompleted
                }).ToList();
        }

        public async Task<SaveLaboratoryResultViewModel> Add(SaveLaboratoryResultViewModel vm)
        {
            LaboratoryResult labResult = new()
            {
                AppointmentId = vm.AppointmentId,
                LaboratoryTestId = vm.LaboratoryTestId,
                Resultado = vm.Resultado,
                IsCompleted = vm.IsCompleted
            };

            await _labResultRepository.AddAsync(labResult);

            // Randevu durumu güncelleniyor
            var appointment = await _appointmentRepository.GetByIdAsync(vm.AppointmentId);
            if (appointment != null)
            {
                appointment.Status = (int)Status.ResultsPending;
                await _appointmentRepository.UpdateAsync(appointment);
            }

            return vm;
        }

        public async Task Update(SaveLaboratoryResultViewModel vm)
        {
            var labResult = await _labResultRepository.GetByIdAsync(vm.Id);
            if (labResult != null)
            {
                labResult.Resultado = vm.Resultado;
                labResult.IsCompleted = vm.IsCompleted;
                await _labResultRepository.UpdateAsync(labResult);
            }

            // Randevu durumu güncelleniyor
            var appointment = await _appointmentRepository.GetByIdAsync(vm.AppointmentId);
            if (appointment != null)
            {
                appointment.Status = (int)Status.ResultsPending;
                await _appointmentRepository.UpdateAsync(appointment);
            }
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<SaveLaboratoryResultViewModel> GetByIdSaveViewModel(int id)
        {
            var labResult = await _labResultRepository.GetByIdAsync(id);

            SaveLaboratoryResultViewModel vm = new();
            vm.Id = labResult.Id;
            vm.AppointmentId = labResult.AppointmentId;
            vm.LaboratoryTestId = labResult.LaboratoryTestId;
            vm.Resultado = labResult.Resultado;
            vm.IsCompleted = labResult.IsCompleted;

            return vm;
        }

        public async Task<List<LaboratoryResultViewModel>> GetAllViewModel()
        {
            var list = await _labResultRepository.GetAllWithIncludeAsync(new List<string> { "Appointment", "LaboratoryTest", "Appointment.Patient" });
            return list
                .Where(labVm => !labVm.IsCompleted)
                .Select(labVm => new LaboratoryResultViewModel
                {
                    Id = labVm.Id,
                    AppointmentId = labVm.AppointmentId,
                    PatientName = $"{labVm.Appointment.Patient.FirstName} {labVm.Appointment.Patient.LastName}",
                    PatientIdNumber = labVm.Appointment.Patient.IdNumber,
                    LaboratoryTestId = labVm.LaboratoryTestId,
                    LaboratoryTestName = labVm.LaboratoryTest.Name
                }).ToList();
        }

        public async Task<List<LaboratoryResultViewModel>> GetAllViewModelWithFilter(FilterLabResultViewModel filter)
        {
            var list = await _labResultRepository.GetAllWithIncludeAsync(new List<string> { "Appointment", "LaboratoryTest", "Appointment.Patient" });
            var listViewModel = list
                .Select(labVm => new LaboratoryResultViewModel
                {
                    Id = labVm.Id,
                    AppointmentId = labVm.AppointmentId,
                    PatientName = $"{labVm.Appointment.Patient.FirstName} {labVm.Appointment.Patient.LastName}",
                    PatientIdNumber = labVm.Appointment.Patient.IdNumber,
                    LaboratoryTestId = labVm.LaboratoryTestId,
                    LaboratoryTestName = labVm.LaboratoryTest.Name,
                    IsCompleted = labVm.IsCompleted
                }).ToList();

            if (filter.LabSearch != null)
            {
                string searchValue = filter.LabSearch.ToUpper();
                listViewModel = listViewModel.Where(labVm => labVm.PatientIdNumber.ToUpper().Contains(searchValue)).ToList();
            }

            if (filter.AppointmentId != 0)
            {
                int appointmentId = filter.AppointmentId;
                listViewModel = listViewModel.Where(labVm => labVm.AppointmentId == appointmentId).ToList();
            }

            return listViewModel;
        }

        public async Task<LaboratoryResultViewModel> GetByAppointmentId(int appointmentId)
        {
            var labResult = await _labResultRepository.GetByIdAsync(appointmentId);

            LaboratoryResultViewModel vm = new();
            vm.Id = labResult.Id;
            vm.AppointmentId = labResult.AppointmentId;
            vm.PatientName = $"{labResult.Appointment.Patient.FirstName} {labResult.Appointment.Patient.LastName}";
            vm.PatientIdNumber = labResult.Appointment.Patient.IdNumber;
            vm.LaboratoryTestId = labResult.LaboratoryTestId;
            vm.LaboratoryTestName = labResult.LaboratoryTest.Name;
            vm.IsCompleted = labResult.IsCompleted;

            return vm;
        }

        public async Task<LaboratoryResultViewModel> GetById(int id)
        {
            var labResult = await _labResultRepository.GetByIdAsync(id);

            LaboratoryResultViewModel vm = new();
            vm.Id = labResult.Id;
            vm.AppointmentId = labResult.AppointmentId;
            vm.PatientName = $"{labResult.Appointment.Patient.FirstName} {labResult.Appointment.Patient.LastName}";
            vm.PatientIdNumber = labResult.Appointment.Patient.IdNumber;
            vm.LaboratoryTestId = labResult.LaboratoryTestId;
            vm.LaboratoryTestName = labResult.LaboratoryTest.Name;
            vm.IsCompleted = labResult.IsCompleted;

            return vm;
        }
    }
}
