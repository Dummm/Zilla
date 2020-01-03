using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Zilla.Models
{
    public class Project
    {
        public Project()
        {
            Members = new HashSet<ApplicationUser>();
            Organizers = new HashSet<ApplicationUser>();
            Assignments = new HashSet<Assignment>();
        }

        [Key]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Please provide a team title")]
        public string Title { get; set; }

        public string Description { get; set; }

        //[ForeignKey("Id")]
        public virtual ICollection<ApplicationUser> Members { get; set; }

        //[ForeignKey("Id")]
        public virtual ICollection<ApplicationUser> Organizers { get; set; }

        //[ForeignKey("AssignmentId")]
        public virtual ICollection<Assignment> Assignments { get; set; }
    }

    public class MembersViewModel
    {
        [Display(Name = "Project")]
        public Project Project { get; set; }

        [Display(Name = "Members")]
        public IEnumerable<ApplicationUser> Members { get; set; }
    }

    public class AddMemberViewModel
    {
        [Display(Name = "Project")]
        public Project Project { get; set; }

        [Display(Name = "Added members")]
        public List<string> AddedMembers { get; set; }

        [Display(Name = "Users")]
        public IEnumerable<SelectListItem> Users { get; set; }
    }

    public class RemoveMemberViewModel
    {
        [Display(Name = "Project")]
        public Project Project { get; set; }

        [Display(Name = "Member")]
        public ApplicationUser User { get; set; }
    }

    public class OrganizersViewModel
    {
        [Display(Name = "Project")]
        public Project Project { get; set; }

        [Display(Name = "Organizers")]
        public IEnumerable<ApplicationUser> Organizers { get; set; }
    }

    public class AddOrganizerViewModel
    {
        [Display(Name = "Project")]
        public Project Project { get; set; }

        [Display(Name = "Added organizers")]
        public List<string> AddedOrganizers { get; set; }

        [Display(Name = "Users")]
        public IEnumerable<SelectListItem> Users { get; set; }
    }

    public class RemoveOrganizerViewModel
    {
        [Display(Name = "Project")]
        public Project Project { get; set; }

        [Display(Name = "Member")]
        public ApplicationUser User { get; set; }
    }

    public class AddAssignmentViewModel
    {
        [Display(Name = "Project")]
        public Project Project { get; set; }

        [Display(Name = "Assignment")]
        public Assignment Assignment { get; set; }

        [Display(Name = "Asignee")]
        public string Asignee{ get; set; }

        [Display(Name = "Members")]
        public IEnumerable<SelectListItem> Members{ get; set; }

    }

}