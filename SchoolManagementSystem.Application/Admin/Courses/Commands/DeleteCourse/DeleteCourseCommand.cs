using MediatR;
using SchoolManagementSystem.Application.Common.Responses;

namespace SchoolManagementSystem.Application.Admin.Courses.Commands.DeleteCourse
{
    public class DeleteCourseCommand : IRequest<ResponseDto<bool>>
    {
        public int Id { get; set; }
        public string AdminId { get; set; }
    }
}