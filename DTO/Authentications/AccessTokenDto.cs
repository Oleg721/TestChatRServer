using System;

namespace DTO.Authentications
{
    public class AccessTokenDto
    {
        public string Value { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
