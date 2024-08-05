using GulDiyet.Core.Domain.Common;
using System;
using System.Collections.Generic;

namespace GulDiyet.Core.Domain.Entities
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public int DiyetisyenId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }

        // Navigation property
        public Diyetisyen Diyetisyen { get; set; }
        public ICollection<Evaluation> Evaluations { get; set; }
    }
}
