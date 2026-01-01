namespace SchoolManagementSystem.Domain.Models.Base
{
    public class BaseNamedEntity : BaseEntity
    {
        public string Name { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
