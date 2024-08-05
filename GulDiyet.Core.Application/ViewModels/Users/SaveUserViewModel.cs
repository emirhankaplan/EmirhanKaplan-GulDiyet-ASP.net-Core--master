using GulDiyet.Core.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace GulDiyet.Core.Application.ViewModels.Users
{
    public class SaveUserViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Kullanıcı adı")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor")]
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Şifreyi doğrulayın")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Ad")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Soyad")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "E-posta")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} zorunludur")]
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }
        [Range(0, maximum: int.MaxValue, ErrorMessage = "Lütfen bir {0} seçin")]
        [Display(Name = "Kullanıcı türü")]
        public Roles? TypeUserId { get; set; }
    }
}
