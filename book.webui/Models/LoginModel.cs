using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace book.webui.Models
{
    public class LoginModel
    {
              [Required]
        [DataType(DataType.EmailAddress)]
        // public string UserName { get; set; }
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Passwords { get; set; }
        public string? ReturnUrl { get; set; }


    }
}