﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using NetTools;
using NetworksManagement.Core;
using NetworksManagement.Infrastructure.Utils;
using NetworksManagement.Data.Models;
using NetworksManagement.Extensions;

namespace NetworksManagement.Controllers.Api
{
    public class ToolsController : Controller
    {
        private readonly IDevicesRepository _devicesRepository;
        private readonly IGroupsRepository _groupsRepository;
        private readonly IInterfacesRepository _interfacesRepository;
        private readonly ICommandsRepository _commandsRepository;
        private readonly IDeviceAccountsRepository _accountsRepository;
        private readonly IHelper _helper;
        private readonly Func<string, IDeviceTools> _serviceAccessor;

        public ToolsController(IDevicesRepository devicesRepository, IGroupsRepository groupsRepository,
            ICommandsRepository commandsRepository, IInterfacesRepository interfacesRepository,
            IDeviceAccountsRepository accountsRepository, Func<string, IDeviceTools> serviceAccessor, IHelper helper)
        {
            _devicesRepository = devicesRepository;
            _groupsRepository = groupsRepository;
            _interfacesRepository = interfacesRepository;
            _commandsRepository = commandsRepository;
            _accountsRepository = accountsRepository;
            _helper = helper;
            _serviceAccessor = serviceAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableAdresses(int? id)
        {
            var group = await _groupsRepository.GetAsync(id);

            if (group == null)
                return NotFound();

            var interfaces = _interfacesRepository.GetByGroupId(group.Id);

            var availableList = new List<string>();

            var range = IPAddressRange.Parse(group.IpRange).ToCidrString();

            string subnet = range.Split('/')[1];

            foreach (var ip in IPAddressRange.Parse(group.IpRange))
            {
                if (!interfaces.Any(i => i.Address.Contains(ip.ToString())))
                    availableList.Add(ip.ToString() + "/" + subnet);
            }

            return Ok(availableList);
        }

        [HttpGet]
        public async Task<bool> DeviceStatus(int? id)
        {
            var device = await _devicesRepository.GetAsync(id);

            if (device == null)
                return false;

            foreach (var ethernet in device.Interfaces)
            {
                using (var ping = new Ping())
                {
                    string ip = (ethernet.Address.Contains("/")) ? ethernet.Address.Split('/')[0] : ethernet.Address;
                    PingReply pingReply = await ping.SendPingAsync(ip, 1000);

                    if (pingReply.Status == IPStatus.Success)
                        return true;
                }
            }

            return false;
        }

        [HttpGet]
        public async Task<float> DevicePing(int? id)
        {
            var device = await _devicesRepository.GetAsync(id);

            if (device == null)
                return -1;

            foreach (var ethernet in device.Interfaces)
            {
                using (var ping = new Ping())
                {
                    string ip = (ethernet.Address.Contains("/")) ? ethernet.Address.Split('/')[0] : ethernet.Address;
                    PingReply pingReply = await ping.SendPingAsync(ip, 1000);

                    if (pingReply.Status == IPStatus.Success)
                        return pingReply.RoundtripTime;
                }
            }

            return -1;
        }

        [HttpGet]
        public async Task<IActionResult> DeviceUptime(int? id)
        {
            var device = await _devicesRepository.GetAsync(id);

            if (device == null)
                return Json("unknown");

            (bool, string) result = _serviceAccessor("M").ExecuteSSHCommand(device, _commandsRepository.GetDeviceUptime(),
                "admin", "");

            return Json((result.Item1 == true) ? result.Item2.Replace("\r\n", "") : "unknown");
        }

        [HttpGet]
        public async Task<IActionResult> AutoUpdate(int? id)
        {
            var device = await _devicesRepository.GetAsync(id);

            if (device == null)
                return NotFound();

            (bool, string) result = _serviceAccessor("M").ExecuteSSHCommand(device, _commandsRepository.RunAutoUpdate(),
                "admin", "");

            return RedirectToAction("Index", "Devices", new { message = result.Item2 });
        }

        [HttpGet]
        public async Task<IActionResult> Backup(int? id)
        {
            var device = await _devicesRepository.GetAsync(id);

            if (device == null)
                return NotFound();

            List<string> cmdList = _commandsRepository.GetBackupScript(device);

            foreach (var cmd in cmdList)
            {
                _serviceAccessor("M").ExecuteSSHCommand(device, cmd, "admin", "");
            }

            return RedirectToAction("Index", "Devices", new { message = "Backup task executed" });
        }

        [HttpPost]
        public async Task<IActionResult> RunCommand(int? deviceId, string commandTxt, string username, string password)
        {
            var device = await _devicesRepository.GetAsync(deviceId);

            if (device == null)
                return NotFound();

            (bool, string) result = _serviceAccessor("M").ExecuteSSHCommand(device, commandTxt, username, password);

            return RedirectToAction("Index", "Devices", new { message = result.Item2 });
        }

        [HttpPost]
        public IActionResult ApplySetting(Device device, List<string> InterfacesNames, List<string> InterfacesAddresses)
        {
            List<Interface> interfaces = _helper.GetInterfacesFromNameAddress(InterfacesNames, InterfacesAddresses);
            device.Interfaces = interfaces;

            List<string> cmdList = _commandsRepository.GetCmdList(device);

            foreach(var cmd in cmdList)
            {
                _serviceAccessor("M").ExecuteSSHCommand(device, cmd, "admin", "");
            }

            return RedirectToAction("Index", "Devices", new { message = "Settings Applied" });
        }

        [HttpPost]
        public async Task<IActionResult> DeviceVersion([FromBody] Device device)
        {
            var deviceFromDb = await _devicesRepository.GetAsync(device.Id);

            if (deviceFromDb == null)
                return NotFound();

            (bool, string) result = _serviceAccessor("M").ExecuteSSHCommand(deviceFromDb, 
                _commandsRepository.GetDeviceVersion(),"admin", "");

            if (result.Item1 == true)
                deviceFromDb.Version = result.Item2;

            await _devicesRepository.UpdateAsync(deviceFromDb);

            return RedirectToAction("Index", "Devices", new { message = result });
        }

        [HttpPost]
        public async Task<IActionResult> DeviceUser(string username, string group ,string password, int deviceId)
        {
            var deviceFromDb = await _devicesRepository.GetAsync(deviceId);

            if (deviceFromDb == null)
                return NotFound();

            (bool, string) result = _serviceAccessor("M").ExecuteSSHCommand(deviceFromDb,
                _commandsRepository.AddDeviceUser(username, group, password), "admin", "");

            if (result.Item1 == true)
            {
                result.Item2 = "User added successfully";

                Enum.TryParse("Active", out Permissions permission);
                await _accountsRepository.AddAccountAsync(new DeviceAccount
                {
                    DeviceId = deviceId,
                    UserName = username,
                    Password = password,
                    Permission = permission
                });
            }

            return RedirectToAction("Details", "Devices", new { id = deviceFromDb.Id, message = result.Item2 });
        }
    }
}