using Contracts;
using DTO.Contracts;

namespace TestChatR.Models
{
    public class MessageVM : BaseEntity<int>
    {
        public string Сontent { get; set; }
        public string GroupName { get; set; }
        public int UserId { get; set; }
    }
}
