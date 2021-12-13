using AutoMapper;
using Contracts.Authentification;
using DAL.Models;
using DTO.Authentications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Requests;
using Models.Responses;
using System.Threading.Tasks;

namespace TestChatR.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IAuthentificationService _authentificationService;
        public AuthController(
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAuthentificationService authentificationService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _authentificationService = authentificationService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticatedUserResponse>> Registration(RegisterRequest registerRequest)
        {
            var registrationDto = _mapper.Map<RegisterRequest, RegisterDto>(registerRequest);
            var registerResult =  await _authentificationService.RegistrationAsync(registrationDto);
            if (!registerResult.Succeeded)
            {
                return BadRequest(registerResult.Error);
            }

            return Ok("its Ok");
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticatedUserResponse>> Login(LoginRequest loginRequest)
        {
            var loginDto = _mapper.Map<LoginRequest, LoginDto>(loginRequest);
            var result = await _authentificationService.LoginAsinc(loginDto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Error);
            }
            var authenticatedUserDto = _mapper.Map<AuthenticatedUserDto, AuthenticatedUserResponse>(result.Payload);
            return Ok(authenticatedUserDto);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<string>> LogOut()
        {
            await _signInManager.SignOutAsync();


            return Ok("SignOut");
        }

        [Authorize(Roles = "admin")]
        [HttpPost("admintest")]
        public async Task<ActionResult<string>> AdminTest(string text)
        {
            return Ok($"ADMIN_TEST======={text}");
        }

        [Authorize(Roles = "user")]
        [HttpPost("usertest")]
        public async Task<ActionResult<string>> UserTest(string text)
        {
            return Ok($"USER_TEST======={text}");
        }

        [Authorize(Roles = "user")]
        [HttpPost("test")]
        public async Task<ActionResult<string>> Test(string text)
        {
            var user = await _userManager.FindByNameAsync(text);
            if (user == null)
            {
                return BadRequest("Login is wrong");
            }
            var i = await _userManager.CreateSecurityTokenAsync(user);
            var i2 =await _userManager.GetAuthenticatorKeyAsync(user);
            var i3 =await _userManager.GetClaimsAsync(user);

            return Ok($"TEST======={text}");
        }
    }

}
