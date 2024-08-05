using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GulDiyet.Core.Application.ViewModels.Report
{
    public class DoctorPerformanceReportViewModel
    {
        public int DoctorId { get; set; }
        public int TotalAppointments { get; set; }
        public double AverageRating { get; set; }
        public List<string> Feedbacks { get; set; }
    }
}
