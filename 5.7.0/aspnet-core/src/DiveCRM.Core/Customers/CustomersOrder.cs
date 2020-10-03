using DiveCRM.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DiveCRM.Customers
{
    [Table("ProCustomerOrder")]
    public class CustomersOrder
    {
        [Key]
        public long Id { get; set; }
        public long? CustomerId { get; set; }

        public string CustomerName { get; set; }
        public DateTime OrderTime { get; set; }
        public int PoolType { get; set; }
        public int CourseType { get; set; }
        public int CoachType { get; set; }
        public string CoachName { get; set; }
        public long? ResponsiblePersonId { get; set; }
        public int Location { get; set; }
        public int State { get; set; } 
        public string Remark { get; set; }
        public DateTime CreateTime { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("ResponsiblePersonId")]
        public virtual User ResponsiblePerson { get; set; }


        [NotMapped]
        public string CustomerName_Display { get; set; }
    }
}
