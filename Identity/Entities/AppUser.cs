using Microsoft.AspNetCore.Identity;

namespace Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public virtual List<IdentityRole> Roles { get; set; }
    }
}
