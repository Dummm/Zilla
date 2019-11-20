using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Zilla.Models
{
    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "baga titlu")]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public virtual ApplicationUser Assigner { get; set; }
        [Required]
        public virtual ApplicationUser Assignee { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual Project Project { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}