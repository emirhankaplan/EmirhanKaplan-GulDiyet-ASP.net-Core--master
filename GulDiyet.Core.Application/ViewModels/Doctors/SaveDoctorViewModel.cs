
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GulDiyet.Core.Application.ViewModels.Diyetisyens
{
    public class SaveDiyetisyenViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        public string FirstName { get; set; }
        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        public string LastName { get; set; }
        [EmailAddress]
        [Display(Name = "E-posta")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        public string Email { get; set; }
        [Phone]
        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        public string Phone { get; set; }
        [Display(Name = "Kimlik Numarası")]
        [Required(ErrorMessage = "{0} alanı zorunludur")]
        public string IdNumber { get; set; }
        public string? ImageUrl { get; set; }
        [DataType(DataType.Upload)]
        [Display(Name = "Doktorun Fotoğrafı")]
        public IFormFile? File { get; set; }
    }
}