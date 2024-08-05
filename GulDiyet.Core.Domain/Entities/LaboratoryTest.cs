using GulDiyet.Core.Domain.Common;

using GulDiyet.Core.Domain.Common;
using System.Collections.Generic;

namespace GulDiyet.Core.Domain.Entities
{
    public class LaboratoryTest : AuditableBaseEntity
    {
        public string Name { get; set; }

        //navigation property
        public ICollection<LaboratoryResult>? LaboratoryResults { get; set; }
    }
}
