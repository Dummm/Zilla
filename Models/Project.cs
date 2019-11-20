using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Zilla.Models
{
    public class Project
    {
        public Project()
        {
            Organizers = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a title")]
        public string Title { get; set; }

        public string Description { get; set; }

        public virtual Team Team { get; set; }

        public virtual ICollection<ApplicationUser> Organizers { get; set; }
               
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}