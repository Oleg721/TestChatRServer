using DTO;
using System.Threading.Tasks;

namespace Contracts.DAL
{
    public interface IMessageRepository : ICrud<int, MessageDto>
    {
        public Task<int> CreateAsync(MessageDto item);

    }
}
