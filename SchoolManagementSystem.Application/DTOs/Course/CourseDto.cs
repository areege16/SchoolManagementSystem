using SchoolManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.DTOs.Course
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Code { set; get; }
        public string? Description { set; get; }
        public int Credits { get; set; }
        public int DepartmentId { get; set; }
        public List<ClassMiniDto>? Classes { get; set; } = new List<ClassMiniDto>();

    }
}
