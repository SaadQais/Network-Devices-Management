using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetTools;
using NetworksManagement.Core;
using NetworksManagement.Data;
using NetworksManagement.Data.Models;
using NetworksManagement.Data.ViewModels;
using NetworksManagement.Infrastructure.Extensions;

namespace NetworksManagement.Controllers
{
    public class DevicesController : Controller
    {
        private readonly IDevicesRepository _devicesRepository;
        private readonly IGroupsRepository _groupsRepository;
        private readonly IInterfacesRepository _interfacesRepository;
        public readonly Helper _helper;

        [BindProperty]
        public DeviceViewModel DeviceVM { get; set; }

        public DevicesController(IDevicesRepository devicesRepository, IGroupsRepository groupsRepository,
            IInterfacesRepository interfacesRepository)
        {
            _devicesRepository = devicesRepository;
            _groupsRepository = groupsRepository;
            _interfacesRepository = interfacesRepository;
            _helper = new Helper();

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

            DeviceVM.Device = await _devicesRepository.GetAsync(id);

            if (DeviceVM.Device == null)
            {
                return NotFound();
            }

            return View(DeviceVM);
        }

        public async Task<IActionResult> Create()
        {
            DeviceVM.Groups = await _groupsRepository.GetAll().ToListAsync();
            return View(DeviceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,GroupId,Type")] Device device, List<string> InterfacesNames,
            List<string> InterfacesAddresses)
        {
            if (ModelState.IsValid)
            {
                device.Interfaces = _helper.GetInterfacesFromNameAddress(InterfacesNames, InterfacesAddresses);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GroupId,Type")] Device device, List<string> InterfacesNames,
            List<string> InterfacesAddresses)
        {
            if (id != device.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    List<Interface> interfaces = _helper.GetInterfacesFromNameAddress(InterfacesNames, InterfacesAddresses);
                    await _devicesRepository.UpdateAsync(device, interfaces);
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

        public async Task<IActionResult> Delete(int? id)
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

            return View(DeviceVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var device = await _devicesRepository.GetAsync(id);
            await _devicesRepository.RemoveAsync(device);
            return RedirectToAction(nameof(Index));
        }

        private bool DeviceExists(int id)
        {
            return _devicesRepository.Any(id);
        }

        public async Task<IActionResult> GetAvailableAdresses(int? id)
        {
            var group = await _groupsRepository.GetAsync(id);
            
            if (group == null)
                return NotFound();

            var interfaces = _interfacesRepository.GetByGroupId(group.Id);

            var availableList = new List<string>();

            foreach (var ip in IPAddressRange.Parse(group.IpRange))
            {
                if(!interfaces.Any(i => i.Address.Contains(ip.ToString())))
                    availableList.Add(ip.ToString());
            }

            return Json(availableList);
        }

        public async Task<bool> GetDeviceStatus(int? id)
        {
            var device = await _devicesRepository.GetAsync(id);

            if (device == null)
                return false;

            foreach(var ethernet in device.Interfaces)
            {
                using var ping = new Ping();
                PingReply pingReply =  await ping.SendPingAsync(ethernet.Address, 1000);

                if (pingReply.Status == IPStatus.Success)
                    return true;
            }

            return false;
        }
    }
}
