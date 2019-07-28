using NetworksManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Data.ViewModels
{
    public class DeviceModelViewModel
    {
        public DeviceModel Model { get; set; }
        public IEnumerable<DeviceModel> Models { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
