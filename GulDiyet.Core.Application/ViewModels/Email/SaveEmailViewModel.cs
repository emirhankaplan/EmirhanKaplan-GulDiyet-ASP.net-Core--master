using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.ViewModels.Email
{
    public class SaveEmailViewModel
    {
        public int Id { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
