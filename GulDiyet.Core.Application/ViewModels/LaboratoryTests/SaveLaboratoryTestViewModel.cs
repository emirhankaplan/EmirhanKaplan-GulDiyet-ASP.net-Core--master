using System.ComponentModel.DataAnnotations;

namespace GulDiyet.Core.Application.ViewModels.LaboratoryTests
{
    public class SaveLaboratoryTestViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
