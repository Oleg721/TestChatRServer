using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class RegisterRequest
    {

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [DefaultValue(Constants.ROLE_USER)]
        public string UserRole { get; set; } 
    }
}
