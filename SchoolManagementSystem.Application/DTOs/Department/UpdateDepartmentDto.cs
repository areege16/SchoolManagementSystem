using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.DTOs.Department
{
    public class UpdateDepartmentDto 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? HeadOfDepartmentId { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
