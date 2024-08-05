using GulDiyet.Core.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace GulDiyet.Core.Application.ViewModels.Users
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı girmelisiniz")]
        [Display(Name = "Kullanıcı adı")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Şifre girmelisiniz")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

