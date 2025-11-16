using SchoolManagementSystem.Domain.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Models
{
    public class Assignment: BaseEntity
    {
        public string Title { set; get; }
        public string? Description { get; set; }
        public DateTime DueDate { set; get; }

        [ForeignKey("Class")]
        public int ClassId { set; get; }
        public Class Class { set; get; }


        [ForeignKey("CreatedByTeacher")]
        public string CreatedByTeacherId { get; set; }
        public Teacher CreatedByTeacher { get; set; }

        public ICollection<Submission>? Submissions { get; set; }


    }
}
