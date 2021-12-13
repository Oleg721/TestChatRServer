using Microsoft.AspNetCore.Identity;


namespace DAL.Models
{
    public class UserRolerr : IdentityRole<int>
    {
        public UserRolerr(string roleName) : base(roleName) { }
    }
}
