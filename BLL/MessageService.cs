using Contracts.DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Contracts
{
    public class MessageService : IMessageService
    {
        private IMessageRepository _messageRepository;
        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }
        public async Task<int> CreateAsync(MessageDto item)
        {
            var result = await _messageRepository.CreateAsync(item);
            return result;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<MessageDto>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MessageDto> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(MessageDto item)
        {
            throw new NotImplementedException();
        }
    }
}
