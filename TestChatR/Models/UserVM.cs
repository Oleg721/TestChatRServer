using DTO.Contracts;

namespace TestChatR.Models
{
    public class UserVM : BaseEntity<int>
    {
        public string Role { get; set; }
        public string UserName { get; set; }
    }
}
