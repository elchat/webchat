using Microsoft.AspNet.Identity.EntityFramework;

namespace Chat.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }
    }

    public class IdentityModels : IdentityDbContext<ApplicationUser>
    {
        public IdentityModels() : base("ChatDb")
        {
        }

        public static IdentityModels Create()
        {
            return new IdentityModels();
        }
    }
}