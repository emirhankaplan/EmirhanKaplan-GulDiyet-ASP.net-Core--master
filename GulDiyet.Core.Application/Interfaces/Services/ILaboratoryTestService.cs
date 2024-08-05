using System.Collections.Generic;
using System.Threading.Tasks;
using GulDiyet.Core.Application.ViewModels.LaboratoryTests;

namespace GulDiyet.Core.Application.Interfaces.Services
{
    public interface ILaboratoryTestService : IGenericService<SaveLaboratoryTestViewModel, LaboratoryTestViewModel>
    {
        Task<List<TestResultViewModel>> GetTestResults(int testId); // Eksik olan yöntem
        Task<LaboratoryTestViewModel> GetById(int id); // Eksik olan yöntem
    }
}
