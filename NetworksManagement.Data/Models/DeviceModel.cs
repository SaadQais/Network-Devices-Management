using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace NetworksManagement.Data.Models
{
    public class DeviceModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsCore { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
