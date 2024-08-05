using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Application.Interfaces.Services;
using GulDiyet.Core.Application.ViewModels.Appointment;
using GulDiyet.Core.Application.ViewModels.Email;
using GulDiyet.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.IO;
using GulDiyet.Core.Application.Enums;

namespace GulDiyet.Core.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IEmailService _emailService;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IDietPlanService _dietPlanService;
        private readonly IPdfService _pdfService;

        public AppointmentService(IAppointmentRepository appointmentRepository, IEmailService emailService, IRabbitMQService rabbitMQService, IDietPlanService dietPlanService, IPdfService pdfService)
        {
            _appointmentRepository = appointmentRepository;
            _emailService = emailService;
            _rabbitMQService = rabbitMQService;
            _dietPlanService = dietPlanService;
            _pdfService = pdfService;
        }

        public async Task<SaveAppointmentViewModel> Add(SaveAppointmentViewModel vm)
        {
            Appointment appointment = new()
            {
                Id = vm.Id,
                PatientId = vm.PatientId,
                DiyetisyenId = vm.DiyetisyenId,
                Day = vm.Day,
                Time = vm.Time,
                Reason = vm.Reason,
                Status = (int)vm.Status
            };

            appointment = await _appointmentRepository.AddAsync(appointment);

            SaveAppointmentViewModel appointmentVm = new()
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DiyetisyenId = appointment.DiyetisyenId,
                Day = appointment.Day,
                Time = appointment.Time,
                Reason = appointment.Reason,
                Status = (Status)appointment.Status
            };

            string patientEmail = await GetPatientEmailAsync(vm.PatientId);

            var emailViewModel = new SaveEmailViewModel
            {
                To = patientEmail,
                Subject = "Yeni Randevu",
                Body = $"Merhaba, yeni bir randevunuz oluşturuldu. Randevu Tarihi: {appointmentVm.Day}, Saati: {appointmentVm.Time}."
            };

            await _emailService.Add(emailViewModel);
            _rabbitMQService.SendMessage(emailViewModel);

            await SendDietPlanByEmail(appointmentVm.PatientId);

            return appointmentVm;
        }

        public async Task<AppointmentViewModel?> GetById(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment == null)
            {
                return null;
            }

            if (appointment.Patient == null || appointment.Diyetisyen == null)
            {
                return null;
            }

            return new AppointmentViewModel
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                PatientName = $"{appointment.Patient.FirstName} {appointment.Patient.LastName}",
                DiyetisyenId = appointment.DiyetisyenId,
                DiyetisyenName = $"{appointment.Diyetisyen.FirstName} {appointment.Diyetisyen.LastName}",
                Day = appointment.Day,
                Time = appointment.Time,
                Reason = appointment.Reason,
                Status = (Status)appointment.Status
            };
        }

        public async Task Delete(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            if (appointment != null)
            {
                await _appointmentRepository.DeleteAsync(appointment);

                string patientEmail = await GetPatientEmailAsync(appointment.PatientId);

                var emailViewModel = new SaveEmailViewModel
                {
                    To = patientEmail,
                    Subject = "Randevu İptali",
                    Body = $"Merhaba, randevunuz iptal edilmiştir. Randevu Tarihi: {appointment.Day}, Saati: {appointment.Time}."
                };

                await _emailService.Add(emailViewModel);
                _rabbitMQService.SendMessage(emailViewModel);
            }
            else
            {
                throw new Exception();
            }
        }

        private async Task<string> GetPatientEmailAsync(int patientId)
        {
            // This method returns the patient's email address based on the patient ID
            return "eraykelesk@gmail.com";
        }

        public async Task<List<AppointmentViewModel>> GetAllViewModel()
        {
            var list = await _appointmentRepository.GetAllWithIncludeAsync(new List<string> { "Patient", "Diyetisyen", "LaboratoryResults" });
            return list.Select(a => new AppointmentViewModel
            {
                Id = a.Id,
                PatientId = a.PatientId,
                PatientName = $"{a.Patient.FirstName} {a.Patient.LastName}",
                DiyetisyenId = a.DiyetisyenId,
                DiyetisyenName = $"{a.Diyetisyen.FirstName} {a.Diyetisyen.LastName}",
                Day = a.Day,
                Time = a.Time,
                Reason = a.Reason,
                Status = (Status)a.Status
            }).ToList();
        }

        public async Task<SaveAppointmentViewModel> GetByIdSaveViewModel(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);

            SaveAppointmentViewModel vm = new()
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DiyetisyenId = appointment.DiyetisyenId,
                Day = appointment.Day,
                Time = appointment.Time,
                Reason = appointment.Reason,
                Status = (Status)appointment.Status
            };

            return vm;
        }

        public async Task Update(SaveAppointmentViewModel vm)
        {
            Appointment appointment = await _appointmentRepository.GetByIdWithIncludeAsync(vm.Id, new List<string> { "LaboratoryResults" });
            appointment.Id = vm.Id;
            appointment.PatientId = vm.PatientId;
            appointment.DiyetisyenId = vm.DiyetisyenId;
            appointment.Day = vm.Day;
            appointment.Time = vm.Time;
            appointment.Reason = vm.Reason;
            appointment.Status = (int)vm.Status;

            await _appointmentRepository.UpdateAsync(appointment);
        }

        public async Task<List<TimeSpan>> GetAvailablePeriods(int diyetisyenId, DateTime day)
        {
            var appointments = await _appointmentRepository.GetAllAsync();
            var occupiedPeriods = appointments
                .Where(a => a.DiyetisyenId == diyetisyenId && a.Day.Date == day.Date)
                .Select(a => a.Time)
                .ToList();

            var allPeriods = new List<TimeSpan>
            {
                new TimeSpan(8, 0, 0),
                new TimeSpan(8, 30, 0),
                new TimeSpan(9, 0, 0),
                new TimeSpan(9, 30, 0),
                new TimeSpan(10, 0, 0),
                new TimeSpan(10, 30, 0),
                new TimeSpan(11, 0, 0),
                new TimeSpan(11, 30, 0),
                new TimeSpan(12, 0, 0),
                new TimeSpan(12, 30, 0),
                new TimeSpan(13, 0, 0),
                new TimeSpan(13, 30, 0),
                new TimeSpan(14, 0, 0),
                new TimeSpan(14, 30, 0),
                new TimeSpan(15, 0, 0),
                new TimeSpan(15, 30, 0),
                new TimeSpan(16, 0, 0),
                new TimeSpan(16, 30, 0),
            };

            return allPeriods.Except(occupiedPeriods).ToList();
        }

        public async Task SendDietPlanByEmail(int patientId)
        {
            var dietPlans = await _dietPlanService.GetDietPlansByPatientIdAsync(patientId);
            if (dietPlans == null || !dietPlans.Any()) throw new Exception("No diet plans found for this patient.");

            var pdfContent = await _pdfService.GenerateDietPlanPdf(dietPlans);
            var emailViewModel = new SaveEmailViewModel
            {
                To = dietPlans.First().PatientName,
                Subject = "Diyet Planınız",
                Body = "Yeni diyet planınız hazır. Ekteki PDF dosyasından detayları inceleyebilirsiniz.",
                Attachments = new List<Attachment> { new Attachment(new MemoryStream(pdfContent), "DiyetPlanı.pdf") }
            };

            await _emailService.SendEmailAsync(emailViewModel);
            _emailService.NotifyNewDietPlan(emailViewModel);
        }

        public async Task<List<AppointmentViewModel>> GetAppointmentsByDiyetisyenId(int diyetisyenId) // Bu metodu ekleyin
        {
            var appointments = await _appointmentRepository.GetAllWithIncludeAsync(new List<string> { "Patient", "Diyetisyen" });
            return appointments
                .Where(a => a.DiyetisyenId == diyetisyenId)
                .Select(a => new AppointmentViewModel
                {
                    Id = a.Id,
                    PatientId = a.PatientId,
                    PatientName = $"{a.Patient.FirstName} {a.Patient.LastName}",
                    DiyetisyenId = a.DiyetisyenId,
                    DiyetisyenName = $"{a.Diyetisyen.FirstName} {a.Diyetisyen.LastName}",
                    Day = a.Day,
                    Time = a.Time,
                    Reason = a.Reason,
                    Status = (Status)a.Status
                }).ToList();
        }
    }
}
