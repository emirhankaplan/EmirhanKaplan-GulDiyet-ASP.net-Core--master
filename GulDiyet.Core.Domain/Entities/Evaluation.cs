using System;
using System.Collections.Generic;

namespace GulDiyet.Core.Domain.Entities
{
    public class Evaluation
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int TimeSlotId { get; set; }
        public int Rating { get; set; }
        public string Feedback { get; set; }
        public int DoctorId { get; set; } // Yeni eklenen özellik

        // Navigation properties
        public Appointment Appointment { get; set; }
        public TimeSlot TimeSlot { get; set; }
    }
}
