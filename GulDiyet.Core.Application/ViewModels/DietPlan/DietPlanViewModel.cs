using System;
using GulDiyet.Core.Domain.Entities;

namespace GulDiyet.Core.Application.ViewModels.DietPlan
{
    public class DietPlanViewModel
    {
        public int Id { get; set; }
        public int DiyetisyenId { get; set; }
        public int PatientId { get; set; }
        public string PlanDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public string PatientName { get; set; } 
        public DietPlanViewModel(GulDiyet.Core.Domain.Entities.DietPlan dietPlan)
        {
            Id = dietPlan.Id;
            DiyetisyenId = dietPlan.DiyetisyenId;
            PatientId = dietPlan.PatientId;
            PlanDetails = dietPlan.PlanDetails;
            CreatedDate = dietPlan.CreatedDate;
            PatientName = $"{dietPlan.Patient.FirstName} {dietPlan.Patient.LastName}";
        }
    }
}
