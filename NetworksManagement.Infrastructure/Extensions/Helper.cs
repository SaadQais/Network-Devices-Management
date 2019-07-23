using NetworksManagement.Core;
using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Infrastructure.Extensions
{
    public class Helper : IHelper
    {
        public List<Interface> GetInterfacesFromNameAddress(List<string> interfacesNames,
            List<string> interfacesAddresses)
        {
            List<Interface> interfaces = new List<Interface>();

            for (int i = 0; i < interfacesNames.Count; i++)
            {
                interfaces.Add(new Interface
                {
                    Name = interfacesNames[i],
                    Address = interfacesAddresses[i]
                });
            }

            return interfaces;
        }
    }
}
