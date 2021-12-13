using System;


namespace DTO.Authentications
{
    public class RefreshTokenDto
    {
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}
