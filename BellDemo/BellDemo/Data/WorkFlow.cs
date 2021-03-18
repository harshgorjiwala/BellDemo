using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BellDemo.Data
{
    public class WorkFlow
    {
        [Key]
        public int WorkFLowID { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "Full Name is required")]
        [DisplayName("Full Name")]
        public string WorkGiven { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int ServiceCategoryTypeID { get; set; }

        [ForeignKey("ServiceCategoryTypeID")]
        public virtual ServiceCategoryType ServiceCategoryType { get; set; }

        public int ServiceCategoryId { get; set; }

        [ForeignKey("ServiceCategoryId")]
        public virtual ServiceCategory ServiceCategory { get; set; }
    }
}
