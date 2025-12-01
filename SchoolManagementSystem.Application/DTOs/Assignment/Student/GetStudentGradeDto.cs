using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.DTOs.Assignment.Student
{
    public class GetStudentGradeDto
    {
        public int AssignmentId { get; set; }
        public string AssignmentTitle { get; set; }
        public DateTime SubmittedDate { get; set; }
        public double? Grade { get; set; }
        public string? Remarks { get; set; }

    }
}
