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
using NetworksManagement.Infrastructure.Utils;

namespace NetworksManagement.Controllers
{
    [Authorize(Roles = Helper.Admin)]
    public class LocationsController : Controller
    {
        private readonly ILocationsRepository _locationsRepository;
        public LocationsController(ILocationsRepository locationRepository)
        {
            _locationsRepository = locationRepository;
        }

        public async Task<IActionResult> Index()
        {
            var locations = await _locationsRepository.GetAll().ToListAsync();

            ViewBag.Current = "Locations";
            return View(locations);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _locationsRepository.GetAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Location location)
        {
            if (ModelState.IsValid)
            {
                await _locationsRepository.AddAsync(location);

                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _locationsRepository.GetAsync(id);

            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Location location)
        {
            if (id != location.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _locationsRepository.UpdateAsync(location);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.Id))
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
            return View(location);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _locationsRepository.GetAsync(id);

            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _locationsRepository.GetAsync(id);

            await _locationsRepository.RemoveAsync(location);

            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _locationsRepository.Any(id);
        }
    }
}
