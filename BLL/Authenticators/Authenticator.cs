using BLL.TokenGenerators;
using DAL.Models;
using DTO;
using DTO.Authentications;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BLL.Authenticators
{
    public class Authenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly UserManager<User> _userManager;

        public Authenticator(UserManager<User> userManager,
            AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator)

        {
            _userManager = userManager;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }

        public async Task<AuthenticatedUserDto> Authenticate(User user)
        {
            var accessTokenDto = await _accessTokenGenerator.GenerateToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            await _userManager.SetAuthenticationTokenAsync(user, 
                Constants.REFRESH_TOKEN_PROVIDER, 
                Constants.REFRESH_TOKEN_NAME, 
                refreshToken);

            return new AuthenticatedUserDto()
            {
                AccessToken = accessTokenDto.Value,
                AccessTokenExpirationTime = accessTokenDto.ExpirationTime,
                RefreshToken = refreshToken
            };
        }
    }
}
