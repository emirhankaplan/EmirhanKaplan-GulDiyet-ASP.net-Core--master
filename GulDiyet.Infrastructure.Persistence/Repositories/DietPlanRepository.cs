using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Domain.Entities;
using GulDiyet.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GulDiyet.Infrastructure.Persistence.Repositories
{
    public class DietPlanRepository : GenericRepository<DietPlan>, IDietPlanRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DietPlanRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<DietPlan>> GetDietPlansByPatientIdAsync(int patientId)
        {
            return await _dbContext.Set<DietPlan>()
                                   .Where(dp => dp.PatientId == patientId)
                                   .ToListAsync();
        }
        public async Task<List<DietPlan>> GetDietPlansByDiyetisyenIdAsync(int diyetisyenId) // Eksik olan yöntem
        {
            return await _dbContext.Set<DietPlan>().Where(dp => dp.DiyetisyenId == diyetisyenId).ToListAsync();
        }

    }
}