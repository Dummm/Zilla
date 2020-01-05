using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public ApplicationUser()
        {
            MemberInProjects = new HashSet<Project>();
            OrganizerInProjects = new HashSet<Project>();
        }

        /*[Required(ErrorMessage = "Please provide a display name.")]*/
        /*public string DisplayName { get; set; }*/
        public string Description { get; set; }
        public SqlDateTime RegistrationDate { get; set; }

        //[ForeignKey("ProjectId")]
        [InverseProperty("Members")]
        public virtual ICollection<Project> MemberInProjects { get; set; }
        [InverseProperty("Organizers")]
        public virtual ICollection<Project> OrganizerInProjects { get; set; }

        [InverseProperty("Assigner")]
        public virtual ICollection<Assignment> AssignerInAssignments { get; set; }
        [InverseProperty("Assignee")]
        public virtual ICollection<Assignment> AssigneeInAssignments { get; set; }

        public string Avatar { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("Avatar", Avatar));
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /// One to many
            /*
            modelBuilder.Entity<Project>()
                .HasMany(d => d.Assignments)
                .WithRequired(f => f.Project);
             */

            modelBuilder.Entity<Assignment>()
                .HasMany(a => a.Comments)
                .WithRequired(c => c.Assignment)
                .WillCascadeOnDelete(true);

            /*
            modelBuilder.Entity<Assignment>()
                .HasOptional(a => a.Assignee)
                .WithMany(u => u.AssigneeInAssignments)
                .HasForeignKey(b => new { b.Assignee_Id })
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Assignment>()
                .HasOptional(a => a.Assigner)
                .WithMany(u => u.AssignerInAssignments)
                .HasForeignKey(b => new { b.Assigner_Id })
                .WillCascadeOnDelete(false);
                */
            /*
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.AssignerInAssignments)
                .WithOptional(a => a.Assigner)
                .HasForeignKey(a => a.Assigner_Id)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.AssigneeInAssignments)
                .WithOptional(a => a.Assignee)
                .HasForeignKey(a => a.Assignee_Id)
                .WillCascadeOnDelete(false);
                */


            /// Many to many
            /*
            */
            modelBuilder.Entity<Project>()
                .HasMany(d => d.Members)
                .WithMany(f => f.MemberInProjects)
                .Map(w => w
                    .ToTable("ProjectsMembers")
                    .MapLeftKey("ProjectId")
                    .MapRightKey("Id"));

            /*
                    */
            modelBuilder.Entity<Project>()
                .HasMany(d => d.Organizers)
                .WithMany(f => f.OrganizerInProjects)
                .Map(w => w
                    .ToTable("ProjectsOrganizers")
                    .MapLeftKey("ProjectsId")
                    .MapRightKey("ApplicationUserId"));
        }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Project> Projects { get; set; }
        //public DbSet<ApplicationUser> Members { get; set; }
        //public DbSet<ApplicationUser> Organizers { get; set; }


    }

}