using DAL.Models;
using DTO;
using DTO.Authentications;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.TokenGenerators
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;
        private UserManager<User> _userManager;

        public AccessTokenGenerator(AuthenticationConfiguration configuration,
            TokenGenerator tokenGenerator, 
            UserManager<User> userManager)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
        }

        public async Task<AccessTokenDto> GenerateToken(User user)
        {

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var userRolesClame2 = await _userManager.GetRolesAsync(user);

            var userRolesClame = (await _userManager.GetRolesAsync(user))
                .Select(role => new Claim(ClaimTypes.Role, role));
            claims.AddRange(userRolesClame);

            DateTime expirationTime = DateTime.UtcNow.AddMinutes(_configuration.AccessTokenExpirationMinutes);
            string value = _tokenGenerator.GenerateToken(
                _configuration.AccessTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                expirationTime,
                claims);

            return new AccessTokenDto()
            {
                Value = value,
                ExpirationTime = expirationTime
            };
        }
    }
}
