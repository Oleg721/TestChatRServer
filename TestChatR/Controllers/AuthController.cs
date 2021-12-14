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
using Microsoft.EntityFrameworkCore;

namespace TestChatR.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AuthController : ControllerBase
    {
        private IMapper _mapper;
        private SignInManager<User> _signInManager;
        private IAuthentificationService _authentificationService;


        private DbContext _context;
        public AuthController(
            IMapper mapper,
            SignInManager<User> signInManager,
            IAuthentificationService authentificationService,
            DbContext context)
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _authentificationService = authentificationService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticatedUserResponse>> Registration(RegisterRequest registerRequest)
        {
            var registrationDto = _mapper.Map<RegisterRequest, RegisterDto>(registerRequest);
            var registerResult =  await _authentificationService.RegistrationAsync(registrationDto);
            if (!registerResult.Succeeded)
            {
                return BadRequest(registerResult.Error);
            }
            var loginDto = _mapper.Map<RegisterRequest, LoginDto>(registerRequest);
            var result = await _authentificationService.LoginAsinc(loginDto);
            var authenticatedUserDto = _mapper.Map<AuthenticatedUserDto, AuthenticatedUserResponse>(result.Payload);
            return Ok(authenticatedUserDto);
        }

        [AllowAnonymous]
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

        [HttpPost("logout")]
        public async Task<ActionResult<string>> LogOut()
        {
            await _signInManager.SignOutAsync();


            return Ok("SignOut");
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequest refreshRequest)
        {
            var authenticatedUserDto = await _authentificationService.Refresh(refreshRequest.RefreshToken);
            if(authenticatedUserDto == null)
            {
                return BadRequest("not result");
            }

            return Ok(authenticatedUserDto);
        }

        ///////////////////////////////////////////////////////////////////////////////////
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
 
        [HttpPost]
        [Route("test")]
        public async Task<ActionResult<string>> Test(string text)
        {
            return Ok($"TEST======={text}");
        }

        [AllowAnonymous]
        [HttpPost("t")]
        public async Task<ActionResult<string>> T(string text)
        {

            return Ok($"T======={text}");
        }
    }

}
