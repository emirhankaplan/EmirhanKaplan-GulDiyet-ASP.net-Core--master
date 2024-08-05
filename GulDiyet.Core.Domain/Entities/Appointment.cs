using GulDiyet.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace GulDiyet.Core.Domain.Entities
{
    public class Appointment : AuditableBaseEntity
    {
        public int PatientId { get; set; }
        public int DiyetisyenId { get; set; }
        public DateTime Day { get; set; }
        public TimeSpan Time { get; set; }
        public string Reason { get; set; }
        public int Status { get; set; }
        public int? Rating { get; set; }
        public string? Feedback { get; set; }

        // Navigation properties
        public Patient? Patient { get; set; }
        public Diyetisyen? Diyetisyen { get; set; }
        public ICollection<LaboratoryResult>? LaboratoryResults { get; set; }
        public ICollection<Evaluation>? Evaluations { get; set; } // Eklenen navigasyon özelliği
    }
}
