using System.Data.Entity;
using Chat.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Chat.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }
    }

    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }

        public ApplicationContext() : base("ChatDb")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationContext, Migrations.Configuration>());
        }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }
    }
}