using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Models
{
    public class Submission
    {
        public int Id { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string? OriginalFileName { get; set; }
        public string? StoredFileName { get; set; }
        public double? Grade { get; set; }
        public string? Remarks { get; set; }

        [ForeignKey("Assignment")]
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; }

        [ForeignKey("Student")]
        public string StudentId { set; get; }
        public Student Student { set; get; }

        [ForeignKey("GradedByTeacher")]
        public string? GradedByTeacherId { get; set; }
        public Teacher GradedByTeacher { get; set; }

    }
}