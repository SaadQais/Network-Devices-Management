using System;
using System.Collections.Generic;
using System.Text;

namespace NetworksManagement.Data.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LocationsGroups> LocationsGroups { get; set; }
    }
}
