
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApplication1.Models
{
    [Table("ProCustomer")]
    public class Customer
    {
        [Key]
        public long Id { get; set; }
        public long Uid { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreateTime { get; set; }
    }
}
