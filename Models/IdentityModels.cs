using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Zilla.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        /*[Required(ErrorMessage = "Please provide a display name.")]
        public string DisplayName { get; set; }*/
        public string Description { get; set; }
        public SqlDateTime RegistrationDate { get; set; }

        /*public ICollection<Project> Projects { get; set; }*/
        /*public ICollection<Task> CreatedTasks { get; set; }*/
        public ICollection<Team> Teams { get; set; }
        public ICollection<ProjectTask> AssignedTasks { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Zilla.Models.ProjectTask> ProjectTasks { get; set; }

        public System.Data.Entity.DbSet<Zilla.Models.Comment> Comments { get; set; }

        public System.Data.Entity.DbSet<Zilla.Models.Project> Projects { get; set; }

        public System.Data.Entity.DbSet<Zilla.Models.Team> Teams { get; set; }
    }
}