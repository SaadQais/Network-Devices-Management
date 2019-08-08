using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetworksManagement.Data;
using NetworksManagement.Data.Models;
using NetworksManagement.Data.ViewModels;
using NetworksManagement.Infrastructure.Utils;

namespace NetworksManagement.Controllers
{
    [Authorize(Roles = Helper.Admin)]
    public class ManageUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public UserViewModel UserVM { get; set; }

        public ManageUsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

            UserVM = new UserViewModel
            {
                Groups = _context.Groups.ToList()
            };
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Current = "ManageUsers";

            var users = await _context.ApplicationUsers.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            UserVM.ApplicationUser = await _context.ApplicationUsers.FindAsync(id);

            if (UserVM.ApplicationUser == null)
                return NotFound();

            UserVM.SelectedGroups = _context.ApplicationUserGroups.Where(group => group.UserId
                == UserVM.ApplicationUser.Id).ToList();

            UserVM.ApplicationUser.SuperAdmin =
                await _userManager.IsInRoleAsync(UserVM.ApplicationUser, Helper.Admin);

            return View(UserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserViewModel user, int[] SelectedGroups)
        {
            if (id != user.ApplicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userFromDb = await _context.ApplicationUsers.FindAsync(id);

                userFromDb.Name = user.ApplicationUser.Name;
                userFromDb.PhoneNumber = user.ApplicationUser.PhoneNumber;

                UpdateUserTowers(SelectedGroups, user.ApplicationUser.Id);

                await UpdateUserRoleAsync(user.ApplicationUser, userFromDb);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers.FindAsync(id);

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var userFromDb = await _context.ApplicationUsers.FindAsync(id);

            if (userFromDb != null)
            {
                userFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        private void UpdateUserTowers(int[] SelectedGroups, string userId)
        {
            var userGroups = _context.ApplicationUserGroups.Where(u => u.UserId == userId).ToList();

            _context.RemoveRange(userGroups);

            foreach (int groupId in SelectedGroups)
            {
                var tower = _context.Groups.FirstOrDefault(t => t.Id == groupId);
                if (tower != null)
                {
                    _context.ApplicationUserGroups.Add(new ApplicationUserGroups
                    {
                        UserId = userId,
                        GroupId = groupId
                    });
                }
            }
        }

        private async Task UpdateUserRoleAsync(ApplicationUser newUser, ApplicationUser oldUser)
        {
            if (newUser.SuperAdmin)
            {
                if (!await _userManager.IsInRoleAsync(newUser, Helper.Admin))
                {
                    await _userManager.AddToRoleAsync(oldUser, Helper.Admin);
                    await _userManager.RemoveFromRoleAsync(oldUser, Helper.User);
                }
            }
            else
            {
                if (await _userManager.IsInRoleAsync(newUser, Helper.Admin))
                {
                    await _userManager.AddToRoleAsync(oldUser, Helper.User);
                    await _userManager.RemoveFromRoleAsync(oldUser, Helper.Admin);
                }
            }
        }

    }
}