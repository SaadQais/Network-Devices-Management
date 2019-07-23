using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Core
{
    public interface IHelper
    {
        public List<Interface> GetInterfacesFromNameAddress(List<string> interfacesNames,
            List<string> interfacesAddresses);
    }
}
