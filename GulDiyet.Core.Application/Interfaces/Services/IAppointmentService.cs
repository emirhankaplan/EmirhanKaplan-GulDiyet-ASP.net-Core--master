using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.Appointment;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IAppointmentService
    {
        Task<SaveAppointmentViewModel> Add(SaveAppointmentViewModel vm);
        Task<AppointmentViewModel?> GetById(int id);
        Task Delete(int id);
        Task<List<AppointmentViewModel>> GetAllViewModel();
        Task<SaveAppointmentViewModel> GetByIdSaveViewModel(int id); // Bu metodu ekleyin
        Task Update(SaveAppointmentViewModel vm);
        Task<List<TimeSpan>> GetAvailablePeriods(int diyetisyenId, DateTime day);
        Task SendDietPlanByEmail(int patientId);
        Task<List<AppointmentViewModel>> GetAppointmentsByDiyetisyenId(int diyetisyenId); // Bu metodu ekleyin
    }
}
