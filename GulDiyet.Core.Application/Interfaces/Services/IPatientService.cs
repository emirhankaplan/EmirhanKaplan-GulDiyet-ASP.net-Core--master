using System.Collections.Generic;
using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.Appointment;
using GulDiyet.Core.Application.ViewModels.Patients;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface IPatientService
    {
        Task<SavePatientViewModel> Add(SavePatientViewModel vm);
        Task Update(SavePatientViewModel vm);
        Task Delete(int id);
        Task<SavePatientViewModel> GetByIdSaveViewModel(int id);
        Task<SavePatientViewModel> GetById(int id); // Eksik olan yöntem
        Task<List<PatientViewModel>> GetAllViewModel();
        Task<List<AppointmentViewModel>> GetAppointmentsByPatientId(int patientId);
    }

}
