using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace GulDiyet.Core.Application.ViewModels.Patients
{
    public class SavePatientViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        public string FirstName { get; set; }
        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        public string LastName { get; set; }
        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        public string Phone { get; set; }
        [Display(Name = "Adres")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        public string Email { get; set; }
        [Display(Name = "Kimlik Numarası")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        public string IdNumber { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime DateBirth { get; set; }
        [Display(Name = "Sigara İçiyor mu?")]
        public bool IsSmoker { get; set; } = false;
        [Display(Name = "Alerjisi Var mı?")]
        public bool HasAllergies { get; set; } = false;
        public string? ImageUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Hasta Fotoğrafı")]
        public IFormFile? File { get; set; }

    }
}
