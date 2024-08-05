using GulDiyet.Core.Application.Interfaces.Repositories;
using GulDiyet.Core.Domain.Entities;
using GulDiyet.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GulDiyet.Infrastructure.Persistence.Repositories
{
    public class TimeSlotRepository : GenericRepository<TimeSlot>, ITimeSlotRepository
    {
        private readonly ApplicationDbContext _context;

        public TimeSlotRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<TimeSlot>> GetAllWithIncludeAsync(List<string> includes)
        {
            var query = _context.Set<TimeSlot>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }
    }
}
