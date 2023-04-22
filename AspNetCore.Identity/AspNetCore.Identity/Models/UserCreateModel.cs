using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.Models
{
    public class UserCreateModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı Gereklidir!")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email Gereklidir!")]
        [EmailAddress(ErrorMessage = "Lütfen bir Email formatı giriniz.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Parola Gereklidir!")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage ="Parolalar eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Cinsiyet alanı boş bırakılamaz!")]
        public string Gender { get; set; }
    }
}
