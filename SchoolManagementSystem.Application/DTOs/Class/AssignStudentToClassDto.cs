using SchoolManagementSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Application.DTOs.Class
{
    public class AssignStudentToClassDto
    {
        public string StudentId { set; get; }
        public int ClassId { set; get; }
        public DateTime EnrollmentDate { set; get; }
    }
}
