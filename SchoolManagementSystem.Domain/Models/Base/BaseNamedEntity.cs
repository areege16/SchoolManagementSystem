using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Models.Base
{
    public class BaseNamedEntity:BaseEntity
    {
        public string Name { get; set; }
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
