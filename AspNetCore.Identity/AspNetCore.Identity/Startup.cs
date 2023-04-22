using AspNetCore.Identity.Context;
using AspNetCore.Identity.CustomDescriber;
using AspNetCore.Identity.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Identity
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Identity Konfigürasyonlarý
            services.AddIdentity<AppUser, AppRole>(opt=>
            {
                // Identity paketinin þifremiz için zorunlu tuttuðu özellik kýsýtlarýný kaldýrýyorum.
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 1; // Default olarak þifre en az 6 karakter olmalýydý. Ben bunu 1 e indiriyorum.
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                //opt.SignIn.RequireConfirmedEmail = true;
                //opt.SignIn.RequireConfirmedPhoneNumber = true;
                opt.Lockout.MaxFailedAccessAttempts = 3; // Kullanýcý 3 kere þifresini yanlýþ girerse!
            }).AddErrorDescriber<CustomErrorDescriber>().AddEntityFrameworkStores<UdemyContext>(); // Identity paketi konfigürasyonu

            // Cookie Konfigürasyonlarý
            services.ConfigureApplicationCookie(opt =>
            {
                //opt.Cookie.HttpOnly = true;
                //opt.Cookie.SameSite = SameSiteMode.Strict;
                //opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                opt.Cookie.Name = "HergeleCookie";
                opt.ExpireTimeSpan = TimeSpan.FromDays(1); // Giriþ yapan kullanýcý bilgisi 1 gün boyunca tutulur.
                opt.LoginPath = new PathString("/Home/SignIn");
                opt.AccessDeniedPath = new PathString("/Home/AccessDenied");
            });

            services.AddDbContext<UdemyContext>(opt =>
            {
                opt.UseSqlServer("server=(localdb)\\mssqllocaldb; database=IdentityDb; integrated security=true;");
            });
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles(); // wwwroot'u dýþarý açýyoruz.
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/node_modules",
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"node_modules"))
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
