using GulDiyet.Core.Domain.Common;
using System;

namespace GulDiyet.Core.Domain.Entities
{
    public class DietPlan : AuditableBaseEntity
    {
        public int DiyetisyenId { get; set; }
        public int PatientId { get; set; }
        public string PlanDetails { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public Diyetisyen Diyetisyen { get; set; }
        public Patient Patient { get; set; }
    }
}
