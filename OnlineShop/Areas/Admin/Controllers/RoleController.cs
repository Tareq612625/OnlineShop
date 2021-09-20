using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        ApplicationDbContext _db;
        UserManager<IdentityUser> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManage, ApplicationDbContext db, UserManager<IdentityUser> userManager) {
            _roleManager = roleManage;
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }
        public async Task<IActionResult> Create() {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string name) {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(role.Name);
            if (isExist) {
                ViewBag.RoleMessage = "This role is already exist";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded) {
                TempData["Save"] = "User role has been create successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(role.Name);
            if (isExist)
            {
                ViewBag.RoleMessage = "This role is already exist";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                TempData["update"] = "User role has been update successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id, string name)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["delete"] = "User role has been delete successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        //User Asign 
        public async Task<IActionResult> Assign() {
            ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(c=>c.LockoutEnd<DateTime.Now||c.LockoutEnd==null).ToList(), "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Assign(RoleUserVm roleUser) {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == roleUser.UserId);

            var existAssignCheck = await _userManager.IsInRoleAsync(user, roleUser.RoleId);
            if (existAssignCheck) {
                ViewBag.RoleAssignMessage = "This user already have a role";
                ViewData["UserId"] = new SelectList(_db.ApplicationUsers.Where(c => c.LockoutEnd < DateTime.Now || c.LockoutEnd == null).ToList(), "Id", "UserName");
                ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
                return View();
            }

            var role = await _userManager.AddToRoleAsync(user, roleUser.RoleId);
            if (role.Succeeded) {
                TempData["Save"] = "User role assign successfully";
                return RedirectToAction(nameof(Assign));
            }
            return View();
        }
        public ActionResult AssignUserRole() {

            var result = from ur in _db.UserRoles
                         join r in _db.Roles on ur.RoleId equals r.Id
                         join u in _db.ApplicationUsers on ur.UserId equals u.Id
                         select new UserRoleMaping()
                         {
                             UserId=ur.UserId,
                             RoleId=ur.RoleId,
                             FirstName=u.FirstName,
                             LastName=u.LastName,
                             UserName=u.UserName,
                             RoleName=r.Name
                         };
            ViewBag.UserRoles = result;

            return View();
        }

    }
}
