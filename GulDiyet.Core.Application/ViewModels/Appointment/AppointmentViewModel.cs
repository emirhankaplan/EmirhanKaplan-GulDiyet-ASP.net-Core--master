using GulDiyet.Core.Application.Enums;
using System;

namespace GulDiyet.Core.Application.ViewModels.Appointment
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int DiyetisyenId { get; set; }
        public string DiyetisyenName { get; set; }
        public DateTime Day { get; set; }
        public TimeSpan Time { get; set; }
        public string Reason { get; set; }
        public Status Status { get; set; }
        public int? Rating { get; set; }
        public string? Feedback { get; set; }
    }
}