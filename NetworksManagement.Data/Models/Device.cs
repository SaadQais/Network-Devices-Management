using NetworksManagement.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetworksManagement.Data.Models
{
    [Serializable]
    public class Device
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Group")]
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
        
        public int DeviceModelId { get; set; }
        [ForeignKey("DeviceModelId")]
        public virtual DeviceModel DeviceModel { get; set; }

        public DeviceType Type { get; set; }

        public virtual ICollection<Interface> Interfaces { get; set; }
    }
}
