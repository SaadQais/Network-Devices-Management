using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetworksManagement.Data.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; } 

        public virtual ICollection<Interface> Interfaces { get; set; }
    }
}
