using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.Models
{
    public class RoleCreateModel
    {
        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        public string Name { get; set; }
        public string Create { get; set; }
    }
}
