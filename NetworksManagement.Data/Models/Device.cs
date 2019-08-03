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

        public string Version { get; set; }

        [Display(Name = "Group")]
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group Group { get; set; }
        
        [Display(Name = "Model")]
        public int ModelId { get; set; }
        [ForeignKey("ModelId")]
        public virtual DeviceModel Model { get; set; }

        public DeviceType Type { get; set; }

        public virtual ICollection<Interface> Interfaces { get; set; }
    }
}
