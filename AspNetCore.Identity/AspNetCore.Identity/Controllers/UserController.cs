using AspNetCore.Identity.Context;
using AspNetCore.Identity.Entities;
using AspNetCore.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Identity.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UdemyContext _context;

        public UserController(UserManager<AppUser> userManager, UdemyContext context, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task< IActionResult >Index()
        {
            //var query = _userManager.Users;

            //// Linq, sorgu mekanizmasını kullanarak kendi listeleme sorgumuzu yazıyoruz.
            //var users = _context.Users.Join(_context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new
            //{
            //    user,
            //    userRole
            //}).Join(_context.Roles,two => two.userRole.RoleId,role=>role.Id, (two,role) => new {two.user, two.userRole, role}).Where(x => x.role.Name != "Admin").Select(x => new AppUser
            //{
            //    Id = x.user.Id,
            //    AccessFailedCount = x.user.AccessFailedCount,
            //    ConcurrencyStamp = x.user.ConcurrencyStamp,
            //    Email = x.user.Email,
            //    EmailConfirmed = x.user.EmailConfirmed,
            //    Gender = x.user.Gender,
            //    ImagePath = x.user.ImagePath,
            //    LockoutEnabled = x.user.LockoutEnabled,
            //    NormalizedEmail = x.user.NormalizedEmail,
            //    NormalizedUserName = x.user.NormalizedUserName,
            //    PasswordHash = x.user.PasswordHash,
            //    PhoneNumber = x.user.PhoneNumber,
            //    UserName = x.user.UserName,
            //}).ToList();

            //// Identity paketi ile gelen hazır sorgu paketini kullanarak tek seferde çağırma işlemi yapıyoruz. (Yukarıdaki ile bu aynı mantıkta çalışıyor. İkiside yalnızca Üyeleri getiriyor.)
            ////var users2 = await _userManager.GetUsersInRoleAsync("Member");

            //return View(users);

            List<AppUser> filteredUsers = new List<AppUser>();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains("Admin"))
                {
                    filteredUsers.Add(user);
                }
            }

            return View(filteredUsers);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            _userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AssignRole(int id)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();
            
            RoleAssignSendModel model = new RoleAssignSendModel();

            List<RoleAssignListModel> list = new List<RoleAssignListModel>();

            foreach (var role in roles)
            {
                list.Add(new()
                {
                    Name = role.Name,
                    RoleId = role.Id,
                    Exist = userRoles.Contains(role.Name)
                });
            }

            model.Roles = list;
            model.UserId = id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(RoleAssignSendModel model)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user); // Kullanıcının rollerini getiriyoruz.

            foreach (var role in model.Roles)
            {
                if (role.Exist)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
