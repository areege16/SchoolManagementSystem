using SchoolManagementSystem.Domain.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class Department : BaseNamedEntity
    {
        public string? Description { get; set; }

        [ForeignKey("HeadOfDepartment")]
        public string? HeadOfDepartmentId { get; set; }
        public Teacher? HeadOfDepartment { get; set; }
        public ICollection<Course>? Courses { get; set; }
    }
}