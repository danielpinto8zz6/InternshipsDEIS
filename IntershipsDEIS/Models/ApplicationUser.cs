using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace IntershipsDEIS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual Company Company { get; set; }
        public virtual Student Student { get; set; }
        public virtual Professor Professor { get; set; }
        public virtual string Roles { get; set; }
    }
}