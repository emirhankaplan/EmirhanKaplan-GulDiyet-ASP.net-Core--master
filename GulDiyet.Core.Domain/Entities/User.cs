using GulDiyet.Core.Domain.Common;

using GulDiyet.Core.Domain.Common;
using System.Collections.Generic;

namespace GulDiyet.Core.Domain.Entities
{
    public class User : AuditableBaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public int TypeUserId { get; set; }

        //navigation properties
        public ICollection<Patient>? Patients { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
        public ICollection<LaboratoryResult>? LaboratoryResults { get; set; }
    }
}