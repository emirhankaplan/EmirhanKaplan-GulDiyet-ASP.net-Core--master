using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Domain.Entities;
using GulDiyet.Infrastructure.Persistence.Contexts;

namespace GulDiyet.Infrastructure.Persistence.Repositories
{
    public class LaboratoryResultRepository : GenericRepository<LaboratoryResult>, ILaboratoryResultRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LaboratoryResultRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }
    }
}
