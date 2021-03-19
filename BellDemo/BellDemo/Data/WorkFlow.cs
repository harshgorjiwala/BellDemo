using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BellDemo.Data
{
    public class WorkFlow
    {
        [Key]
        public int WorkFLowID { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "Full Work Name is required")]
        [DisplayName("Full Work Name")]
        public string WorkGiven { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [Required]
        public int ServiceCategoryTypeID { get; set; }

        [ForeignKey("ServiceCategoryTypeID")]
        public virtual ServiceCategoryType ServiceCategoryType { get; set; }

        public int ServiceCategoryId { get; set; }

        [ForeignKey("ServiceCategoryId")]
        public virtual ServiceCategory ServiceCategory { get; set; }

        //todo If I would have more time then I would create DTO for all these entities and used that DTO on UI and I can also used method extension to map DTO with entities 
        //so as hack I had to add NotMapped property in here
        [NotMapped]
        public IList<SelectListItem> ServiceCategoryTypes { get; set; }

        [NotMapped]
        public IList<SelectListItem> ServiceCategories { get; set; }
    }
}
