using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.DTOs.Course
{
   public class CreateCourseDto
    {
        public string Name { get; set; }          
        public string Code { get; set; }          
        public string? Description { get; set; } 
        public int Credits { get; set; }         
        public int DepartmentId { get; set; }     
    }
}
