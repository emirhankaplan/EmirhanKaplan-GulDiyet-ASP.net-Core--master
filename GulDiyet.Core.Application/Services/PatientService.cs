using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Appointment;
using GulDiyet.Core.Application.ViewModels.Patients;
using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            this._patientRepository = patientRepository;
        }

        public async Task<SavePatientViewModel> Add(SavePatientViewModel vm)
        {
            Patient patient = new();
            patient.FirstName = vm.FirstName;
            patient.LastName = vm.LastName;
            patient.Phone = vm.Phone;
            patient.Email = vm.Email;
            patient.IdNumber = vm.IdNumber;
            patient.DateBirth = vm.DateBirth;
            patient.IsSmoker = vm.IsSmoker;
            patient.HasAllergies = vm.HasAllergies;
            patient.ImagePath = vm.ImageUrl;

            patient = await _patientRepository.AddAsync(patient);

            SavePatientViewModel patientVm = new();
            patientVm.Id = patient.Id;
            patientVm.FirstName = patient.FirstName;
            patientVm.LastName = patient.LastName;
            patientVm.Phone = patient.Phone;
            patientVm.Email = patient.Email;
            patientVm.IdNumber = patient.IdNumber;
            patientVm.DateBirth = patient.DateBirth;
            patientVm.IsSmoker = patient.IsSmoker;
            patientVm.HasAllergies = patient.HasAllergies;
            patientVm.ImageUrl = patient.ImagePath;

            return patientVm;
        }
        public async Task<SavePatientViewModel> GetById(int id) // Eksik olan yöntem
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            SavePatientViewModel vm = new();
            vm.Id = patient.Id;
            vm.FirstName = patient.FirstName;
            vm.LastName = patient.LastName;
            vm.Phone = patient.Phone;
            vm.Email = patient.Email;
            vm.IdNumber = patient.IdNumber;
            vm.DateBirth = patient.DateBirth;
            vm.IsSmoker = patient.IsSmoker;
            vm.HasAllergies = patient.HasAllergies;
            vm.ImageUrl = patient.ImagePath;

            return vm;
        }
        public async Task Update(SavePatientViewModel vm)
        {
            Patient patient = await _patientRepository.GetByIdAsync(vm.Id);

            patient.Id = vm.Id;
            patient.FirstName = vm.FirstName;
            patient.LastName = vm.LastName;
            patient.Phone = vm.Phone;
            patient.Email = vm.Email;
            patient.IdNumber = vm.IdNumber;
            patient.DateBirth = vm.DateBirth;
            patient.IsSmoker = vm.IsSmoker;
            patient.HasAllergies = vm.HasAllergies;
            patient.ImagePath = vm.ImageUrl;

            await _patientRepository.UpdateAsync(patient);
        }

        public async Task Delete(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);
            await _patientRepository.DeleteAsync(patient);
        }

        public async Task<SavePatientViewModel> GetByIdSaveViewModel(int id)
        {
            var patient = await _patientRepository.GetByIdAsync(id);

            SavePatientViewModel vm = new();
            vm.Id = patient.Id;
            vm.FirstName = patient.FirstName;
            vm.LastName = patient.LastName;
            vm.Phone = patient.Phone;
            vm.Email = patient.Email;
            vm.IdNumber = patient.IdNumber;
            vm.DateBirth = patient.DateBirth;
            vm.IsSmoker = patient.IsSmoker;
            vm.HasAllergies = patient.HasAllergies;
            vm.ImageUrl = patient.ImagePath;

            return vm;
        }

        public async Task<List<PatientViewModel>> GetAllViewModel()
        {
            var list = await _patientRepository.GetAllAsync();
            return list.Select(p => new PatientViewModel
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Phone = p.Phone,
                Address = p.Email,
                IdNumber = p.IdNumber,
                DateBirth = p.DateBirth,
                IsSmoker = p.IsSmoker,
                HasAllergies = p.HasAllergies,
                ImageUrl = p.ImagePath
            }).ToList();
        }

        public async Task<List<AppointmentViewModel>> GetAppointmentsByPatientId(int patientId)
        {
            var appointments = await _patientRepository.GetAppointmentsByPatientIdAsync(patientId);
            return appointments.Select(a => new AppointmentViewModel
            {
                Id = a.Id,
                PatientId = a.PatientId,
                DiyetisyenId = a.DiyetisyenId,
                Day = a.Day,
                Time = a.Time
            }).ToList();
        }
    }
}
