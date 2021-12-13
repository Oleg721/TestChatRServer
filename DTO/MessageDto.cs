﻿using DTO.Contracts;

namespace DTO
{
    public class MessageDto : BaseEntity<int>
    {
        public string Сontent { get; set; }
        public string GroupName { get; set; }
        public int UserId { get; set; }
    }
}
