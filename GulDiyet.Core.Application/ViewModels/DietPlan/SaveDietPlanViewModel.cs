using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace GulDiyet.Core.Application.ViewModels.DietPlan
{
    public class SaveDietPlanViewModel
    {
        public int Id { get; set; }
        public int DiyetisyenId { get; set; }
        public int PatientId { get; set; }
        public string PlanDetails { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
