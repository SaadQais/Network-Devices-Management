using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetworksManagement.Data.Models
{
    public class LocationsGroups
    {
        public int LocationId { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        public int GroupId { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
    }
}
