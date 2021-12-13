using DTO.Contracts;
using Microsoft.AspNetCore.Identity;

namespace DTO
{
    public class UserDto : BaseEntity<int>
    {
        public string UserRole { get; set; } = Constants.ROLE_USER;
        public string UserName { get; set; }
    }
}
