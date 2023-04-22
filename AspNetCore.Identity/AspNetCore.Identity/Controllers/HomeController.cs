using AspNetCore.Identity.Entities;
using AspNetCore.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AspNetCore.Identity.Controllers
{
    [AutoValidateAntiforgeryToken] // Artık bu sunucunun üretmediği X bir noktadan belirli istekler bu sayfaya yapılamaz.
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager; // Kullanıcı oluşturma, silme vs. gibi işlemler için kullanılan Identity Package Class'ı (Example Create Method.)
        private readonly SignInManager<AppUser> _signInManager; // Kullanıcının sisteme giriş yapması, çıkış yapması vs. gibi işlemler için kullanılan Identity Package Class'ı (Example SignIn Method.)
        private readonly RoleManager<AppRole> _roleManager; // Rol ile ilgili işlemleri yapmamız için.

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            //_roleManager.CreateAsync(new()
            //{
            //    Name = "Member",
            //    CreatedTime = System.DateTime.Now,
            //});

            return View();
        }

        public IActionResult Create()
        {
            return View(new UserCreateModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Email = model.Email,
                    Gender = model.Gender,
                    UserName = model.Username,
                };
                // Güvenlik açıklarını engellemek için şifreyi veritabanına kaydetmeden önce özel şifrelemeler yapmamız gerekiyor.
                // CreateAsync methoduna "model.Password" 'u ayrı bir parametre olarak gönderdiğimizde, fonksiyon kullanıcı şifresini özel olarak şifreleyip veritabanına öyle kaydedecek.
                // PasswordHash işlemi
                var identityResult = await _userManager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");

                    return RedirectToAction("Index");
                }

                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public IActionResult SignIn(string returnUrl)
        {


            return View(new UserSignInModel { ReturnUrl = returnUrl});
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);


                var user = await _userManager.FindByNameAsync(model.Username);
                var role = await _userManager.GetRolesAsync(user);
                if (signInResult.Succeeded)
                {
                    if (model.ReturnUrl != null)
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    // giriş işlemi başarılı

                    if(role.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel");
                    }
                    else
                    {
                        return RedirectToAction("Panel");
                    }
                }

                var message = string.Empty;
                if (user != null)
                {
                    var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                    // _userManager.Options.Lockout.MaxFailedAccessAttempts --> Startup.cs 'de konfigürasyon ayarlarından ayarladığımız hata sayısına _userManager'i kullanarak erişebiliyoruz.
                    message = $"{_userManager.Options.Lockout.MaxFailedAccessAttempts - failedCount} kez daha hatalı parola girerseniz hesabınız kitlenecektir.";
                }
                else
                {
                    message = "Kullanıcı adı veya şifre hatalı";
                }

                ModelState.AddModelError("", message);
            }


            

            return View(model);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public IActionResult GetUserInfo() // Kullanıcı bilgilerini görüntüle.
        {
            var userName = User.Identity.Name; // _userManager ile yaptığımız işlemi, async olmadan kullanılıyor. Burası önemli.
            var role = User.Claims.ToString();
            return View();
        }

        [Authorize(Roles ="Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public IActionResult Panel()
        {
            return View();
        }

        [Authorize(Roles = "Member")]
        public IActionResult MemberPage()
        {
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("SignIn");
        }
    }
}
