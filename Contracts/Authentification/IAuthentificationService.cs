using DTO.Authentications;
using System.Threading.Tasks;

namespace Contracts.Authentification
{
    public interface IAuthentificationService
    {
        public Task<IAuthentificationResult<bool>> RegistrationAsync(RegisterDto registerDto);
        public Task<IAuthentificationResult<AuthenticatedUserDto>> LoginAsinc(LoginDto loginDto);
        public Task<AuthenticatedUserDto> Refresh(string refreshToken);
        public Task Logout();

    }
}
