using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Domain.Entities;
using GulDiyet.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace GulDiyet.Infrastructure.Persistence.Repositories
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PatientRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Appointment>> GetAppointmentsByPatientIdAsync(int patientId) // Eksik olan yöntem
        {
            return await _dbContext.Set<Appointment>().Where(a => a.PatientId == patientId).ToListAsync();
        }
    }
}
