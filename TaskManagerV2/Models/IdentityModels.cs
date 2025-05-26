using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace TaskManagerV2.Models
{
    
    public class ApplicationUser : IdentityUser
    {
        public string HomeTown { get; set; }
        public DateTime? BirthDate { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}