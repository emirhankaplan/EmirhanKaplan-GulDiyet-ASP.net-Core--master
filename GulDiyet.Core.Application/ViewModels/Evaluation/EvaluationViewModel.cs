using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace GulDiyet.Core.Application.ViewModels.Evaluation
{
    public class EvaluationViewModel
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }

        [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
        public int Rating { get; set; }

        [Required]
        public string Feedback { get; set; }
    }
}
