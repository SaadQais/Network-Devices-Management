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
    public class DevicesController : Controller
    {
        private readonly IDevicesRepository _devicesRepository;
        private readonly IGroupsRepository _groupsRepository;

        [BindProperty]
        public DeviceViewModel DeviceVM { get; set; }

        public DevicesController(IDevicesRepository devicesRepository, IGroupsRepository groupsRepository)
        {
            _devicesRepository = devicesRepository;
            _groupsRepository = groupsRepository;

            DeviceVM = new DeviceViewModel
            {
                Groups = new List<Group>()
            };
        }

        // GET: Devices
        public async Task<IActionResult> Index()
        {
            var devices = await _devicesRepository.GetAll().ToListAsync();

            return View(devices);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _devicesRepository.GetAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        public async Task<IActionResult> Create()
        {
            DeviceVM.Groups = await _groupsRepository.GetAll().ToListAsync();
            return View(DeviceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,GroupId")] Device device)
        {
            if (ModelState.IsValid)
            {
                await _devicesRepository.AddAsync(device);

                return RedirectToAction(nameof(Index));
            }
            DeviceVM.Groups = await _groupsRepository.GetAll().ToListAsync();
            return View(device);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DeviceVM.Device = await _devicesRepository.GetAsync(id);
            if (DeviceVM.Device == null)
            {
                return NotFound();
            }
            DeviceVM.Groups = await _groupsRepository.GetAll().ToListAsync();

            return View(DeviceVM);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GroupId")] Device device)
        //{
        //    if (id != device.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(device);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!DeviceExists(device.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["GroupId"] = new SelectList(_context.Groups, "Id", "Id", device.GroupId);
        //    return View(device);
        //}

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var device = await _context.Devices
        //        .Include(d => d.Group)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (device == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(device);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var device = await _context.Devices.FindAsync(id);
        //    _context.Devices.Remove(device);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool DeviceExists(int id)
        //{
        //    return _context.Devices.Any(e => e.Id == id);
        //}
    }
}
