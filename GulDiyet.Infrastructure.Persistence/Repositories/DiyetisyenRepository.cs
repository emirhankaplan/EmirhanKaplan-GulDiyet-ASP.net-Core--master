using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Domain.Entities;
using GulDiyet.Infrastructure.Persistence.Contexts;

namespace GulDiyet.Infrastructure.Persistence.Repositories
{
    public class DiyetisyenRepository : GenericRepository<Diyetisyen>, IDiyetisyenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DiyetisyenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }
    }
}
