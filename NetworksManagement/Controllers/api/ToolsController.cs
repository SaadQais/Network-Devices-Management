using System;
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
        private readonly IHelper _helper;
        private readonly Func<string, IDeviceTools> _serviceAccessor;

        public ToolsController(IDevicesRepository devicesRepository, IGroupsRepository groupsRepository,
            ICommandsRepository commandsRepository, IInterfacesRepository interfacesRepository, 
            Func<string, IDeviceTools> serviceAccessor, IHelper helper)
        {
            _devicesRepository = devicesRepository;
            _groupsRepository = groupsRepository;
            _interfacesRepository = interfacesRepository;
            _commandsRepository = commandsRepository;
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

            foreach (var ip in range)
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
                    PingReply pingReply = await ping.SendPingAsync(ethernet.Address, 1000);

                    if (pingReply.Status == IPStatus.Success)
                        return true;
                }
            }

            return false;
        }

        [HttpGet]
        public async Task<IActionResult> AutoUpdate(int? id)
        {
            var device = await _devicesRepository.GetAsync(id);

            if (device == null)
                return NotFound();

            string result = _serviceAccessor("M").ExecuteSSHCommand(device, _commandsRepository.RunAutoUpdate(),
                "admin", "");

            return RedirectToAction("Index", "Devices", new { message = result });
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

            string result = _serviceAccessor("M").ExecuteSSHCommand(device, commandTxt, username, password);

            return RedirectToAction("Index", "Devices", new { message = result });
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
    }
}