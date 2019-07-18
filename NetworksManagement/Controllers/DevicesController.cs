using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetTools;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GroupId")] Device device, List<Interface> Interfaces)
        {
            if (id != device.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _devicesRepository.UpdateAsync(device, Interfaces);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeviceExists(device.Id))
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
            DeviceVM.Groups = await _groupsRepository.GetAll().ToListAsync();
            return View(DeviceVM.Groups);
        }

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

        private bool DeviceExists(int id)
        {
            return _devicesRepository.Any(id);
        }

        public async Task<IActionResult> GetAvailableAdresses(int? id)
        {
            var group = await _groupsRepository.GetAsync(id);

            if (group == null)
                return NotFound();

            var availableList = new List<string>();

            foreach (var ip in IPAddressRange.Parse(group.IpRange))
            {
                
                availableList.Add(ip.ToString());
            }

            return Json(availableList);
        }
    }
}
