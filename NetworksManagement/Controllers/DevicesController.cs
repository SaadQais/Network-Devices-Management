﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetworksManagement.Core;
using NetworksManagement.Data.Models;
using NetworksManagement.Data.ViewModels;
using NetworksManagement.Extensions;
using NetworksManagement.Infrastructure.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace NetworksManagement.Controllers
{
    [Authorize]
    public class DevicesController : Controller
    {
        private readonly IDevicesRepository _devicesRepository;
        private readonly IGroupsRepository _groupsRepository;
        private readonly IInterfacesRepository _interfacesRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IDeviceAccountsRepository _accountsRepository;
        public readonly IHelper _helper;

        [BindProperty]
        public DeviceViewModel DeviceVM { get; set; }

        public DevicesController(IDevicesRepository devicesRepository, IGroupsRepository groupsRepository,
            IInterfacesRepository interfacesRepository, IModelRepository modelRepository,
            IDeviceAccountsRepository accountsRepository, IHelper helper)
        {
            _devicesRepository = devicesRepository;
            _groupsRepository = groupsRepository;
            _interfacesRepository = interfacesRepository;
            _modelRepository = modelRepository;
            _accountsRepository = accountsRepository;
            _helper = helper;

            DeviceVM = new DeviceViewModel
            {
                Groups = new List<Group>()
            };
        }

        public async Task<IActionResult> Index(string message, int? page)
        {
            var devices =  _devicesRepository.GetAll();

            if (!User.IsInRole(Helper.Admin))
            {
                var userId = Helper.GetUserId(this.User);
                var userGroups = _groupsRepository.GetUserGroups(userId);

                devices = devices.Where(d => userGroups.Any(g => g.Id == d.GroupId));
            }

            ViewBag.Message = message;
            ViewBag.Current = "Devices";

            int pageSize = 15;

            return View(await devices.ToPagedListAsync(page ?? 1, pageSize));
        }

        public async Task<IActionResult> Details(int? id, string message)
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

            DeviceVM.Accounts = _accountsRepository.GetDeviceAccounts(id.GetValueOrDefault()).ToList();

            ViewBag.Message = message;
            return View(DeviceVM);
        }

        public async Task<IActionResult> Create()
        {
            DeviceVM.Groups = await _groupsRepository.GetAll().ToListAsync();
            DeviceVM.Models = await _modelRepository.GetAll().ToListAsync();

            return View(DeviceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,GroupId,Type,ModelId,IsMonitoring")] Device device, List<string> InterfacesNames,
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
            DeviceVM.Models = await _modelRepository.GetAll().ToListAsync();

            return View(DeviceVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,GroupId,Type,ModelId,IsMonitoring")] Device device, List<string> InterfacesNames,
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
    }
}
