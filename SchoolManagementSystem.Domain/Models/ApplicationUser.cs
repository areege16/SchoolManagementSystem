using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Teacher? Teacher { get; set; }
        public Student? Student { get; set; }
        public Admin? Admin { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}