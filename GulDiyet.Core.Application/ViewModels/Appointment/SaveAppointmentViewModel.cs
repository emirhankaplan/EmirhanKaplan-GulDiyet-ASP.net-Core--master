
using GulDiyet.Core.Application.Enums;
using GulDiyet.Core.Application.ViewModels.Diyetisyens;
using GulDiyet.Core.Application.ViewModels.Patients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GulDiyet.Core.Application.ViewModels.Appointment
{
    public class SaveAppointmentViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DiyetisyenId { get; set; }
        public DateTime Day { get; set; }
        public TimeSpan Time { get; set; }
        public string Reason { get; set; }
        public Status Status { get; set; }
    }


}