using DA_TOTNGHIEP.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DA_TOTNGHIEP.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleManagerController : Controller
    {
        private readonly DA_TOTNGHIEP2Context _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleManagerController(RoleManager<IdentityRole> roleManager, DA_TOTNGHIEP2Context context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.CountProduct = (from ok in _context.Products select ok.Id).Count();
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return RedirectToAction("Index");
        }
    }
}
