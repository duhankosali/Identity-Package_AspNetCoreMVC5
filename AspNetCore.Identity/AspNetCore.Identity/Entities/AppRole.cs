using Microsoft.AspNetCore.Identity;
using System;

namespace AspNetCore.Identity.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public DateTime CreatedTime { get; set; }
    }
}
