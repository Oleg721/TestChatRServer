using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICrud<TId, TDto>
    {
        public Task<List<TDto>> GetAsync();
        public Task<TDto> GetAsync(TId id);
        public Task<bool> DeleteAsync(TId id);
        public Task<bool> UpdateAsync(TDto item);
      //  Task<TId> CreateAsync(TDto item);
    }
}
