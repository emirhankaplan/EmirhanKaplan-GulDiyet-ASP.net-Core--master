using GulDiyet.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GulDiyet.Core.Domain.Common;
using System;

namespace GulDiyet.Core.Domain.Entities
{
    public class Email : AuditableBaseEntity
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentDate { get; set; }
    }
}
