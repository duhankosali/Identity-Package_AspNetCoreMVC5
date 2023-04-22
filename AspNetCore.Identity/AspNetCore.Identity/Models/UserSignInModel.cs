using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.Models
{
    public class UserSignInModel
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Şifre gereklidir.")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
