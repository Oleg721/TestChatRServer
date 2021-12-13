using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestChatR.Models
{
    public class AccessTokenVM
    {
        public string Value { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
