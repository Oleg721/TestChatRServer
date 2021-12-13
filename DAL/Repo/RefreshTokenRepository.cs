using AutoMapper;
using Contracts.Authentification;
using DAL.Models;
using DTO;
using DTO.Authentications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class RefreshTokenRepository : IRefreshTokenBase<RefreshTokenDto>
    {

        protected IdentityDbContext<User, IdentityRole<int>, int> _context;
        protected IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public RefreshTokenRepository(DbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context as IdentityDbContext<User, IdentityRole<int>, int>;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<bool> CreateAsync(RefreshTokenDto refreshToken)
        {
            var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
            var result  = await _userManager.SetAuthenticationTokenAsync(user,
                Constants.REFRESH_TOKEN_PROVIDER,
                Constants.REFRESH_TOKEN_NAME,
                refreshToken.Token);

            return result.Succeeded;
        }

        public async Task<bool> DeleteAsync(RefreshTokenDto refreshToken)
        {
            var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
            var result = await _userManager.RemoveAuthenticationTokenAsync(user,
               Constants.REFRESH_TOKEN_PROVIDER,
               Constants.REFRESH_TOKEN_NAME);

            return result.Succeeded;
        }

        public async Task<RefreshTokenDto> GetByTokenAsync(string token)
        {
            var refreshToken = await _context.UserTokens.FirstOrDefaultAsync<IdentityUserToken<int>>(e => e.Value == token);
            if(refreshToken != null)
            {
                return new RefreshTokenDto() 
                { 
                    Token = refreshToken.Value, 
                    UserId = refreshToken.UserId 
                };
            }
            return null;
        }
    }
}
