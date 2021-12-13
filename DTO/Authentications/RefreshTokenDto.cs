using System;


namespace DTO.Authentications
{
    public class RefreshTokenDto
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
    }
}
