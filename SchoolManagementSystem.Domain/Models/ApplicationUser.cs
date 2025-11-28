using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow; //TODO remove default value 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Teacher? Teacher { get; set; }
        public Student? Student { get; set; }
        public Admin? Admin { get; set; }

    }
}
