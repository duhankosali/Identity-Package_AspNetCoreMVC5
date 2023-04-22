using AspNetCore.Identity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Identity.Context
{
    public class UdemyContext : IdentityDbContext<AppUser, AppRole, int> // IdentityDbContext'e AppUser ve AppRole verdik. Böylece paketin bize sağladığı sınıfları değil kendi oluşturduğumuz sınıflardaki özel değişkenlerimizi de eklemiş olduk. 
    {                                                                    // Örneğin ek olarak fleetId 'yi de ekledik.
        public UdemyContext(DbContextOptions<UdemyContext> options) : base(options)
        {

        }
    }
}
