using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.Interfaces.Repositories
{
    public interface ILaboratoryTestRepository : IGenericRepository<LaboratoryTest>
    {
        Task<List<LaboratoryResult>> GetTestResultsAsync(int testId); // Eksik olan yöntem

    }
}
