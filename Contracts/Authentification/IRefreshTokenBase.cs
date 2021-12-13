using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Authentification
{
    public interface IRefreshTokenBase<TDto> 
    {
        public Task<TDto> GetByTokenAsync(string token);
        public Task<bool> CreateAsync(TDto refreshToken);
        public Task<bool> DeleteAsync(TDto refreshToken);

    }

}
