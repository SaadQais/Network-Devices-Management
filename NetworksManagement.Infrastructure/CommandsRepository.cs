using NetworksManagement.Core;
using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Infrastructure
{
    public class CommandsRepository : ICommandsRepository
    {
        public List<string> GetCmdList(Device device)
        {
            List<string> cmdList = new List<string>();

            foreach(var item in device.Interfaces)
            {
                cmdList.Add(AddIpAddress(item.Address, item.Name));
            }

            cmdList.Add(SetIdentity(device.Name));

            return cmdList;
        }

        public string RunAutoUpdate()
        {
            return $"system package update install ;";
        }

        private string SetIdentity(string identity)
        {
            return $"system identity set name= { identity } ;";
        }

        private string AddGateway(string gateway)
        {
            return $"ip route add gateway= { gateway } ;";
        }

        private string AddIpAddress(string ip, string ether)
        {
            return $"ip address add address= { ip } interface= { ether } ;";
        }

    }
}
