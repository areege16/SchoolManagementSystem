using SchoolManagementSystem.Domain.Models.Base;

namespace SchoolManagementSystem.Domain.Models
{
    public class Notification : BaseEntity
    {
        public string Title { set; get; }
        public bool IsRead { set; get; }
        public string RecipientRole { get; set; }
        public string? RecipientId { get; set; }
        public ApplicationUser? Recipient { get; set; }
        public string Message { get; set; }
    }
}