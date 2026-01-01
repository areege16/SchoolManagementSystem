using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class Teacher
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<Department>? Departments { get; set; }
        public ICollection<Class>? Classes { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
        public ICollection<Assignment>? Assignments { get; set; }
        public ICollection<Submission>? Submissions { get; set; }
    }
}