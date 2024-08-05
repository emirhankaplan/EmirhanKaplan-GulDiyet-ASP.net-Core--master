using GulDiyet.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace GulDiyet.Core.Domain.Entities
{
    public class Patient : AuditableBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public DateTime DateBirth { get; set; } 
        public bool IsSmoker { get; set; }
        public bool HasAllergies { get; set; }
        public string? ImagePath { get; set; }

        // Navigation properties
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<LaboratoryResult>? LaboratoryResults { get; set; }

        // DietPlan ilişkisi
        public ICollection<DietPlan>? DietPlans { get; set; }
    }
}
