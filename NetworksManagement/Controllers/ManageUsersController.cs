using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetworksManagement.Data;
using NetworksManagement.Data.ViewModels;

namespace NetworksManagement.Controllers
{
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
    }
}