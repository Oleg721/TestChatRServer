using AutoMapper;
using BLL.TokenGenerators;
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
        private Authenticator _authenticator;
        public AuthentificationService(
            IMapper mapper,
            UserManager<User> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            SignInManager<User> signInManager,
            Authenticator authenticator,
            AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator)
        {
            _mapper = mapper;
            _userManager = userManager;
            _authenticator = authenticator;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
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

            var authenticatedUserDto = await _authenticator.Authenticate(user);

            return AuthentificationResult<AuthenticatedUserDto>.Success(authenticatedUserDto);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AuthenticatedUserDto> Refresh(string refreshToken)
        {
            var i = await _authenticator.Refresh(refreshToken);
            return i;
        }

    }
}
