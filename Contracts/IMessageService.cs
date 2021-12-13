using Contracts;
using DTO;

namespace BLL.Contracts
{
    public interface IMessageService : ICrud<int, MessageDto>
    {
    }
}
