using System;

namespace DTO.Authentications
{
    public class AuthenticatedUserDto
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpirationTime { get; set; }
        public string RefreshToken { get; set; }
    }
}
