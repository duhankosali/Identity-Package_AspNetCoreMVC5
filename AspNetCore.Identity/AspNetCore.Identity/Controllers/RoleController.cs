using AspNetCore.Identity.Entities;
using AspNetCore.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Identity.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View(new RoleCreateModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new AppRole
                {
                    Name = model.Name,
                    CreatedTime = DateTime.Now,
                });
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(model);
        }
    }
}
