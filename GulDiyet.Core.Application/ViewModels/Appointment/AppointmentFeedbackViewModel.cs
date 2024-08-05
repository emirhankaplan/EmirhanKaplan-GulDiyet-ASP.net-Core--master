using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.ViewModels.Appointment
{
    public class AppointmentFeedbackViewModel
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        [Required(ErrorMessage = "Değerlendirme zorunludur")]
        [Range(1, 10, ErrorMessage = "Değerlendirme 1 ile 10 arasında olmalıdır")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Geri bildirim zorunludur")]
        public string Feedback { get; set; }
    }
}
