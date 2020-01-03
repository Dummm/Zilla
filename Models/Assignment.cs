using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zilla.Models
{   
    public enum Status
    {
        Not_Started,
        In_Progress,
        Completed
    }

    public class Assignment
    {
        public Assignment()
        {
            Comments = new HashSet<Comment>();
        }

        [Key]
        public int AssignmentId { get; set; }

        [Required(ErrorMessage = "baga titlu")]
        public string Title { get; set; }

        public string Description { get; set; }

        public Status Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public virtual ApplicationUser Assigner { get; set; }

        public virtual ApplicationUser Assignee { get; set; }

        //[ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        //[ForeignKey("Id")]
        public virtual ICollection<Comment> Comments { get; set; }
    }

    public class CreateAssignmentViewModel
    {
        [Display(Name = "Assignment")]
        public Assignment Assignment { get; set; }

        [Display(Name = "Added Projects")]
        public string AddedProject{ get; set; }

        [Display(Name = "Projects")]
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}
