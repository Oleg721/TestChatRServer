using AutoMapper;
using Contracts.DAL;
using DAL.Models;
using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public class MessageRepository : BaseRepository<Message, int, MessageDto>, IMessageRepository
    {
        public MessageRepository(DbContext context, IMapper mapper) : base(context, mapper){ }
    }
}
