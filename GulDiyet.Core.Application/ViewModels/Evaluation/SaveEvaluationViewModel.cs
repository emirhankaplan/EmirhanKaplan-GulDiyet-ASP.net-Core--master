using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace GulDiyet.Core.Application.ViewModels.Evaluation
{
    public class SaveEvaluationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        public int AppointmentId { get; set; }

        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
        [Required(ErrorMessage = "Rating is required")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Feedback is required")]
        public string Feedback { get; set; }
    }
}
