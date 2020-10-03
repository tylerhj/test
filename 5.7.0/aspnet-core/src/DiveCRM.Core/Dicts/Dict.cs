using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DiveCRM.Dicts
{
    [Table("Dict")]
    public class Dict
    {
        [Key]
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int? DKey { get; set; }
        public string DValue { get; set; }
        public string DValueEn { get; set; }
    }
}
