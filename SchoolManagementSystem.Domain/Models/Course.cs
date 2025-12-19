using SchoolManagementSystem.Domain.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class Course : BaseNamedEntity
    {
        public string Code { set; get; }
        public string? Description { set; get; }
        public int Credits { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public ICollection<Class>? Classes { get; set; }
    }
}
