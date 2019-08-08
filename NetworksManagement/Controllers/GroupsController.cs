using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetworksManagement.Core;
using NetworksManagement.Data;
using NetworksManagement.Data.Models;
using NetworksManagement.Data.ViewModels;
using NetworksManagement.Infrastructure.Utils;

namespace NetworksManagement.Controllers
{
    [Authorize(Roles = Helper.Admin)]
    public class GroupsController : Controller
    {
        private readonly IGroupsRepository _groupsRepository;
        private readonly ILocationsRepository _locationsRepository;

        [BindProperty]
        public GroupViewModel GroupVM { get; set; }
        public GroupsController(IGroupsRepository groupsRepository, ILocationsRepository locationsRepository)
        {
            _groupsRepository = groupsRepository;
            _locationsRepository = locationsRepository;

            GroupVM = new GroupViewModel
            {
                Group = new Group(),
                Groups = new List<Group>(),
                Locations = _locationsRepository.GetAll().ToList()
            };
        }

        public async Task<IActionResult> Index()
        {
            var groups = await _groupsRepository.GetAll().ToListAsync();
            GroupVM.Groups = groups;

            ViewBag.Current = "Groups";
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
                    await _groupsRepository.UpdateAsync(group, SelectedLocations);
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

            GroupVM.Group = await _groupsRepository.GetAsync(id);

            if (GroupVM.Group == null)
            {
                return NotFound();
            }

            return View(GroupVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var group = await _groupsRepository.GetAsync(id);

            await _groupsRepository.RemoveAsync(group);

            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            return _groupsRepository.Any(id);
        }
    }
}
