using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BellDemo.Data
{
    public class ServiceCategory
    {
        [Key]
        public int ServiceCategoryId { get; set; }

        [Required]
        public string ServiceCategoryName { get; set; }
        
        public virtual ServiceCategoryType ServiceCategoryTypeId { get; set; }

        public virtual ICollection<WorkFlow> WorkFlows { get; set; }
    }
}
