using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Domain.Entities;
using GulDiyet.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GulDiyet.Infrastructure.Persistence.Repositories
{
    public class EvaluationRepository : GenericRepository<Evaluation>, IEvaluationRepository
    {
        private readonly ApplicationDbContext _context;

        public EvaluationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Evaluation>> GetAllWithIncludeAsync(List<string> includes)
        {
            var query = _context.Set<Evaluation>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }

        public async Task<List<Evaluation>> GetEvaluationsByDoctorIdAsync(int doctorId) // Eksik olan yöntem
        {
            return await _context.Evaluations
                .Where(e => e.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<List<Evaluation>> GetEvaluationsByAppointmentIdAsync(int appointmentId) // Eksik olan yöntem
        {
            return await _context.Evaluations
                .Where(e => e.AppointmentId == appointmentId)
                .ToListAsync();
        }
    }
}
