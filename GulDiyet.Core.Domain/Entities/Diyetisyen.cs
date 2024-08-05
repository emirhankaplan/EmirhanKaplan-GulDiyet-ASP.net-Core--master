using GulDiyet.Core.Domain.Common;
using System.Collections.Generic;

namespace GulDiyet.Core.Domain.Entities
{
    public class Diyetisyen : AuditableBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string IdNumber { get; set; }
        public string? ImagePath { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<DietPlan>? DietPlans { get; set; }
    }
}