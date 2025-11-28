using SchoolManagementSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.DTOs.Class
{
    public class UpdateClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Semester? Semester { set; get; }
        public DateOnly? StartDate { set; get; }
        public DateOnly? EndDate { set; get; }
        public DateTime? UpdatedDate { get; set; }
        public int? CourseId { set; get; }
        public string? TeacherId { get; set; }

    }
}
