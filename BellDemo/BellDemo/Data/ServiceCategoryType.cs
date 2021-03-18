using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BellDemo.Data
{
    public class ServiceCategoryType
    {
        [Key]
        public int ServiceCategoryTypeID { get; set; }
        [Required]
        [Column(TypeName ="nvarchar(50)")]
        public String ServiceCategoryTypeName { get; set; }

        public virtual ICollection<WorkFlow> WorkFlows { get; set; }
    }
}
