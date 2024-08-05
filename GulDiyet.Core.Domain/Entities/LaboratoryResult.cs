

using GulDiyet.Core.Domain.Common;

namespace GulDiyet.Core.Domain.Entities
{
    public class LaboratoryResult : AuditableBaseEntity
    {
        public int TestId { get; set; } // Eklenen özellik
        public int AppointmentId { get; set; }
        public int LaboratoryTestId { get; set; }
        public string? Resultado { get; set; }
        public bool IsCompleted { get; set; } = false;

        //navigation properties
        public Appointment? Appointment { get; set; }
        public LaboratoryTest? LaboratoryTest { get; set; }
    }
}
