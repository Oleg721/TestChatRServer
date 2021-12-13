using BLL.TokenGenerators;
using Contracts.Authentification;
using DAL.Models;
using DAL.Repo;
using DTO;
using DTO.Authentications;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BLL.Authenticators
{
    public class Authenticator : IRefreshTokenBase<RefreshTokenDto>
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private RefreshTokenRepository _refreshTokenRepository;
        private readonly UserManager<User> _userManager;

        public Authenticator(AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator,
            RefreshTokenRepository refreshTokenRepository,
            UserManager<User> userManager)

        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
            _userManager = userManager;
        }


        public async Task<AuthenticatedUserDto> Authenticate(User user)
        {
            var accessTokenDto = await _accessTokenGenerator.GenerateToken(user);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            var refreshTokenDto = new RefreshTokenDto()
            {
                UserId = user.Id,
                Token = refreshToken
            };
            var result =  await CreateAsync(refreshTokenDto);
            if (!result)
            {
                return null;
            }

            return new AuthenticatedUserDto()
            {
                AccessToken = accessTokenDto.Value,
                AccessTokenExpirationTime = accessTokenDto.ExpirationTime,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthenticatedUserDto> Refresh(string refreshToken)
        {
            var refreshTokenDto = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
            if(refreshTokenDto == null)
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(refreshTokenDto.UserId.ToString());
            if(user == null)
            {
                return null;
            }

            var isDeleted = await _refreshTokenRepository.DeleteAsync(refreshTokenDto);
            if (!isDeleted)
            {
                return null;
            }

            return await Authenticate(user); ;
        }

        public Task<bool> CreateAsync(RefreshTokenDto refreshToken)
        {
            return _refreshTokenRepository.CreateAsync(refreshToken);
        }

        public Task<bool> DeleteAsync(RefreshTokenDto refreshToken)
        {
            return _refreshTokenRepository.DeleteAsync(refreshToken);
        }

        public Task<RefreshTokenDto> GetByTokenAsync(string token)
        {
            return _refreshTokenRepository.GetByTokenAsync(token);
        }
    }
}
