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

namespace NetworksManagement.Controllers.Api
{
    public class ToolsController : Controller
    {
        private readonly IDevicesRepository _devicesRepository;
        private readonly IGroupsRepository _groupsRepository;
        private readonly IInterfacesRepository _interfacesRepository;
        private readonly MikrotikTools _mikrotik;

        public ToolsController(IDevicesRepository devicesRepository, IGroupsRepository groupsRepository,
            IInterfacesRepository interfacesRepository)
        {
            _devicesRepository = devicesRepository;
            _groupsRepository = groupsRepository;
            _interfacesRepository = interfacesRepository;
            _mikrotik = new MikrotikTools();
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableAdresses(int? id)
        {
            var group = await _groupsRepository.GetAsync(id);

            if (group == null)
                return NotFound();

            var interfaces = _interfacesRepository.GetByGroupId(group.Id);

            var availableList = new List<string>();

            foreach (var ip in IPAddressRange.Parse(group.IpRange))
            {
                if (!interfaces.Any(i => i.Address.Contains(ip.ToString())))
                    availableList.Add(ip.ToString());
            }

            return Ok(availableList);
        }

        [HttpGet]
        public async Task<bool> GetDeviceStatus(int? id)
        {
            var device = await _devicesRepository.GetAsync(id);

            if (device == null)
                return false;

            foreach (var ethernet in device.Interfaces)
            {
                using var ping = new Ping();
                PingReply pingReply = await ping.SendPingAsync(ethernet.Address, 1000);

                if (pingReply.Status == IPStatus.Success)
                    return true;
            }

            return false;
        }

        [HttpPost]
        public async Task<IActionResult> RunCommand(int? deviceId, string commandTxt, string username, string password)
        {
            var device = await _devicesRepository.GetAsync(deviceId);

            if (device == null)
                return NotFound();

            string result = _mikrotik.ExecuteSSHCommand(device, commandTxt, username, password);

            return RedirectToAction("Index", "Devices");
        }
    }
}