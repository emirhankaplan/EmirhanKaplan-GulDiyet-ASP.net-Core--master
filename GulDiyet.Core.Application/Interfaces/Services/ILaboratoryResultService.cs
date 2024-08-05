using System.Collections.Generic;
using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.LaboratoryResult;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface ILaboratoryResultService : IGenericService<SaveLaboratoryResultViewModel, LaboratoryResultViewModel>
    {
        Task<List<LaboratoryResultViewModel>> GetAllViewModelWithFilter(FilterLabResultViewModel filter);
        Task<List<LaboratoryResultViewModel>> GetResultsByAppointmentId(int appointmentId); // Eksik olan yöntem
        Task<LaboratoryResultViewModel> GetByAppointmentId(int appointmentId); // Eksik olan yöntem
        Task<LaboratoryResultViewModel> GetById(int id); // Eksik olan yöntem
    }
}
