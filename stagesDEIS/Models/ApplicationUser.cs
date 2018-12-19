using Microsoft.AspNetCore.Identity;

namespace stagesDEIS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Company Company { get; set; }
    }
}