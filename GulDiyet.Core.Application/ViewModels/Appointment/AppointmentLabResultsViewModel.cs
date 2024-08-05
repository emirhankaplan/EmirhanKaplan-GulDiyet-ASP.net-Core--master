using System.Collections.Generic;
using GulDiyet.Core.Application.ViewModels.LaboratoryResult;

namespace GulDiyet.Core.Application.ViewModels.Appointment
{
    public class AppointmentLabResultsViewModel
    {
        public AppointmentViewModel Appointment { get; set; }
        public List<LaboratoryResultViewModel> LabResults { get; set; }
    }
}
