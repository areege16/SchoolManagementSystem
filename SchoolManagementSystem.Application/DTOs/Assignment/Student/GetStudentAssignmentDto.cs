using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.DTOs.Assignment.Student
{
    public class GetStudentAssignmentDto
    {
        public int AssignmentId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public string ClassName { get; set; }
        public string CourseName { get; set; }
        public string CreatedByTeacherName { get; set; }

    }
}
