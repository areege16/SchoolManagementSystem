using SchoolManagementSystem.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Models
{
    public class Department:BaseNamedEntity
    {
        public string? Description { get; set; }

        [ForeignKey("HeadOfDepartment")]
        public string? HeadOfDepartmentId { get; set; }
        public Teacher? HeadOfDepartment { get; set;}

        public ICollection<Course>? Courses { get; set;  }

    }
}
