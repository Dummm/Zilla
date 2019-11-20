using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        /*public Assignment()
        {
            Comments = new HashSet<Comment>();
        }*/

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "baga titlu")]
        public string Title { get; set; }

        public string Description { get; set; }

        public Status Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public virtual ApplicationUser Assigner { get; set; }

        public virtual ApplicationUser Assignee { get; set; }

        public virtual Project Project { get; set; }
        
        public virtual ICollection<Comment> Comments { get; set; }
    }
}