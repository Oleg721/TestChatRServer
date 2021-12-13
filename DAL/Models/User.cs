using DTO.Contracts;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class User : IdentityUser<int>
    {
        public List<Message> Messages { get; set; }

        [NotMapped]
        public override bool EmailConfirmed { get; set; }
        [NotMapped]
        public override string NormalizedEmail { get; set; }
        [NotMapped]
        public override string Email { get; set; }
        [NotMapped]
        public override string PhoneNumber { get; set; }
    }

}

