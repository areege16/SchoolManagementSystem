using MediatR;
using SchoolManagementSystem.Application.Common.Responses;

namespace SchoolManagementSystem.Application.Teachers.Classes.Commands.DeleteClass
{
    public class DeleteClassCommand : IRequest<ResponseDto<bool>>
    {
        public int Id { set; get; }
        public string TeacherId { set; get; }
    }
}