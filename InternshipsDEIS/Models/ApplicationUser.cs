using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace InternshipsDEIS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public string Role { get; set; }
    }
}