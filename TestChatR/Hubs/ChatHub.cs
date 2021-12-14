using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using BLL.Contracts;
using TestChatR.Models;
using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Authorization;

namespace SignalRApp
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ChatHub : Hub
    {
        private IMessageService _messageService;
        private IMapper _mapper;

        public ChatHub( IMessageService messageService, IMapper mapper)
        {
            Console.WriteLine("New!!!");

            _messageService = messageService;
            _mapper = mapper;
        }

        public async override Task OnConnectedAsync()
        {
            var context = Context;
            var clients = Clients;

            Console.WriteLine();
        }
        public async Task Send(string message, string userName)
        {
            var messageVM = new MessageVM() 
            { 
                UserId =  1, 
                Сontent = message
            };
            var messageDTO = _mapper.Map<MessageDto>(messageVM);

           // await _messageService.CreateAsync(messageDTO);
            await Clients.All.SendAsync("Send", message, userName);
        }
    }
}