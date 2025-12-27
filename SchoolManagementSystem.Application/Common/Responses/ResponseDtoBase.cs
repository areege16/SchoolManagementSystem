using SchoolManagementSystem.Domain.Enums;

namespace SchoolManagementSystem.Application.Common.Responses
{
    public abstract class ResponseDtoBase
    {
        public bool IsSuccess { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public string Message { get; set; }
    }
}