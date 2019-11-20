using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using Zilla.Models;

[assembly: OwinStartupAttribute(typeof(Zilla.Startup))]
namespace Zilla
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateApplicationRoles();
        }
        void CreateApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            AddRole("Administrator", roleManager);
            AddRole("User", roleManager);

            AddUser(
                "admin@admin.com", 
                "admin@admin.com", 
                "Administrator1!", 
                userManager,
                "Administrator"
            );
        }

        void AddRole(string name, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExists(name))
            {
                var role = new IdentityRole();
                role.Name = name;
                roleManager.Create(role);
            }
        }

        void AddUser(string username, string email, string password, UserManager<ApplicationUser> userManager, string role)
        {
            var user = new ApplicationUser();
            user.UserName = username;
            user.Email = email;
            var adminCreated = userManager.Create(user, password);
            if (adminCreated.Succeeded)
            {
                userManager.AddToRole(user.Id, role);
            }
        }
    }
}
