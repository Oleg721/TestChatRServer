using DTO.Contracts;

namespace DAL.Models
{
    public class Message : BaseEntity<int>
    {
        public string Сontent { get; set; }
        public string GroupName { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
