using SchoolManagementSystem.Domain.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class Assignment : BaseEntity
    {
        public string Title { set; get; }
        public string? Description { get; set; }
        public DateOnly DueDate { set; get; }

        [ForeignKey("Class")]
        public int ClassId { set; get; }
        public Class Class { set; get; }

        [ForeignKey("CreatedByTeacher")]
        public string CreatedByTeacherId { get; set; }
        public Teacher CreatedByTeacher { get; set; }
        public ICollection<Submission>? Submissions { get; set; }
    }
}