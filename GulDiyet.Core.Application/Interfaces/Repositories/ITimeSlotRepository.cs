using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GulDiyet.Core.Domain.Entities;

using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.Interfaces.Repositories
{
    public interface ITimeSlotRepository : IGenericRepository<TimeSlot>
    {
        Task<List<TimeSlot>> GetAllWithIncludeAsync(List<string> includes);
    }
}
