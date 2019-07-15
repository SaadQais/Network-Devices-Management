using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Data.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpRange { get; set; }

        public ICollection<LocationsGroups> LocationsGroups { get; set; }
    }
}
