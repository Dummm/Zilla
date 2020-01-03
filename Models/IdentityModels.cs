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
            Projects = new HashSet<Project>();
        }

        /*[Required(ErrorMessage = "Please provide a display name.")]*/
        /*public string DisplayName { get; set; }*/
        public string Description { get; set; }
        public SqlDateTime RegistrationDate { get; set; }

        //[ForeignKey("ProjectId")]
        public virtual ICollection<Project> Projects { get; set; }
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

            /// Many to many
            /*
            */
            modelBuilder.Entity<Project>()
                .HasMany(d => d.Members)
                .WithMany(f => f.Projects)
                .Map(w => w
                    .ToTable("ProjectsMembers")
                    .MapLeftKey("ProjectId")
                    .MapRightKey("Id"));

            // De ce nu merge? da
            /*
            modelBuilder.Entity<Project>()
                .HasMany(d => d.Organizers)
                .WithMany(f => f.Projects)
                .Map(w => w
                    .ToTable("ProjectsOrganizers")
                    .MapLeftKey("ProjectsId")
                    .MapRightKey("ApplicationUserId"));
                    */
        }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Project> Projects { get; set; }
        //public DbSet<ApplicationUser> Members { get; set; }
        //public DbSet<ApplicationUser> Organizers { get; set; }


    }

}