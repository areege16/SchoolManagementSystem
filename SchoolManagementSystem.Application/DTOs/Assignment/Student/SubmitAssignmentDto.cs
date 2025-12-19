using Microsoft.AspNetCore.Http;

namespace SchoolManagementSystem.Application.DTOs.Assignment.Student
{
    public class SubmitAssignmentDto
    {
        public int AssignmentId { get; set; }
        public IFormFile File { get; set; }
    }
}