using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.Entities
{
    public class AppUser : IdentityUser<int> // ApplicationUser
    {
        public string ImagePath { get; set; } // Kullanıcı fotoğrafı
        public string Gender { get; set; } // Cinsiyet
    }
}
