using AutoMapper;
using BLL.Authenticators.TokenGenerators;
using BLL.Utils;
using Contracts;
using Contracts.Authentification;
using DAL.Models;
using DTO.Authentications;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Authenticators
{
    public class AuthentificationService : IAuthentificationService
    {
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private RoleManager<IdentityRole<int>> _roleManager;
        private AccessTokenGenerator _accessTokenGenerator;
        private RefreshTokenGenerator _refreshTokenGenerator;
        private IRefreshTokenBase<RefreshTokenDto> _refreshTokenRepository;

        public AuthentificationService(
            IMapper mapper,
            UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            SignInManager<User> signInManager,
            AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator,
            IRefreshTokenBase<RefreshTokenDto> refreshTokenRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<IAuthentificationResult<bool>> RegistrationAsync(RegisterDto registerDto)
        {
            var role = await _roleManager.FindByNameAsync(registerDto.UserRole);
            if (role == null)
            {
                return AuthentificationResult<bool>.Failed("Wrong role");
            }

            var registrationUser = _mapper.Map<RegisterDto, User>(registerDto);
            var userRresult = await _userManager.CreateAsync(registrationUser, registerDto.Password);
            if (!userRresult.Succeeded)
            {
                var primaryError = userRresult.Errors.FirstOrDefault();
                return AuthentificationResult<bool>.Failed(primaryError.Description);
            }
            var user = await _userManager.FindByNameAsync(registrationUser.UserName);

            var addRoleResalt = await _userManager.AddToRoleAsync(user, registerDto.UserRole);
            if (!addRoleResalt.Succeeded)
            {
                var primaryError = addRoleResalt.Errors.FirstOrDefault();
                await _userManager.DeleteAsync(user);
                return AuthentificationResult<bool>.Failed(primaryError.Description);
            }

            return AuthentificationResult<bool>.Success(true);
        }


        public async Task<IAuthentificationResult<AuthenticatedUserDto>> LoginAsinc(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                return AuthentificationResult<AuthenticatedUserDto>.Failed("Login is wrong");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return AuthentificationResult<AuthenticatedUserDto>.Failed("Login is wrong");
            }

            var authenticatedUserDto = await Authenticate(user);

            return AuthentificationResult<AuthenticatedUserDto>.Success(authenticatedUserDto);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AuthenticatedUserDto> Authenticate(IdentityUser<int> user)
        {
            var accessTokenDto = await _accessTokenGenerator.GenerateToken(user as User);
            string refreshToken = _refreshTokenGenerator.GenerateToken();

            var refreshTokenDto = new RefreshTokenDto()
            {
                UserId = user.Id,
                Token = refreshToken
            };
            var result = await CreateAsync(refreshTokenDto);
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
            if (refreshTokenDto == null)
            {
                return null;
            }

            var user = await _userManager.FindByIdAsync(refreshTokenDto.UserId.ToString());
            if (user == null)
            {
                return null;
            }

            var isDeleted = await _refreshTokenRepository.DeleteAsync(refreshTokenDto);
            if (!isDeleted)
            {
                return null;
            }

            return await Authenticate(user); 
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
