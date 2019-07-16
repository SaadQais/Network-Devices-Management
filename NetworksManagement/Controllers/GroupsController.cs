using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetworksManagement.Core;
using NetworksManagement.Data;
using NetworksManagement.Data.Models;
using NetworksManagement.Data.ViewModels;

namespace NetworksManagement.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IGroupsRepository _groupsRepository;

        [BindProperty]
        public GroupViewModel GroupVM { get; set; }
        public GroupsController(ApplicationDbContext context, IGroupsRepository groupsRepository)
        {
            _context = context;
            _groupsRepository = groupsRepository;

            GroupVM = new GroupViewModel
            {
                Group = new Group(),
                Groups = new List<Group>(),
                Locations = _context.Locations.ToList()
            };
        }

        public async Task<IActionResult> Index()
        {
            var groups = await _groupsRepository.GetAll().ToListAsync();
            GroupVM.Groups = groups;

            return View(GroupVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GroupVM.Group = await _groupsRepository.GetAsync(id);

            if (GroupVM.Group == null)
            {
                return NotFound();
            }

            return View(GroupVM);
        }

        public IActionResult Create()
        {
            return View(GroupVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IpRange")] Group group, int[] SelectedLocations)
        {
            if (ModelState.IsValid)
            {
                await _groupsRepository.AddAsync(group, SelectedLocations);
               
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            GroupVM.Group = await _groupsRepository.GetAsync(id);

            if (GroupVM.Group == null)
            {
                return NotFound();
            }
            return View(GroupVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,IpRange")] Group group, int[] SelectedLocations)
        {
            if (id != group.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(group);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(group.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @group = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@group == null)
            {
                return NotFound();
            }

            return View(@group);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @group = await _context.Groups.FindAsync(id);
            _context.Groups.Remove(@group);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
