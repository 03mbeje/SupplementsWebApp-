using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using GoldenDelight_Suppliments.Models;

[assembly: OwinStartupAttribute(typeof(GoldenDelight_Suppliments.Startup))]
namespace GoldenDelight_Suppliments
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUserAndRoles();
        }

        public void CreateUserAndRoles()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (!roleManager.RoleExists("Administrator"))
            {
                //create super admin role
                var role = new IdentityRole("Administrator");
                roleManager.Create(role);

                //create default user
                var user = new ApplicationUser();
                user.UserName = "goldenD@gmail.com";
                user.Email = "goldenD@gmail.com";
                string pwd = "GoldenDelight";

                var newuser = userManager.Create(user, pwd);
                if (newuser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrator");
                }

            }
        }
    }
}
