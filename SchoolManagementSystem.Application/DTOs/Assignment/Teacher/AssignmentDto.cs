using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.DTOs.Assignment.Teacher
{
    public class AssignmentDto
    {
        public int Id { set; get; }
        public string Title { set; get; }
        public string? Description { get; set; }
        public DateTime DueDate { set; get; }
        public int ClassId { set; get; }
        public string CreatedByTeacherId { get; set; }
        public DateTime CreatedDate { get; set; } 
    }
}
