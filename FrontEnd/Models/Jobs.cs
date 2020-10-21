using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Models
{
    public class Jobs
    {
        [Display(Name = "Id")]
        public int Job { get; set; }
        [Required]
        [Display(Name = "Job Title")]
        [MaxLength(255)]
        public String JobTitle { get; set; }
        [Required]
        [Display(Name = "Description")]
        [MaxLength(255)]
        public String Description { get; set; }
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Expires At")]
        public DateTime ExpiresAt { get; set; }

    }
}
