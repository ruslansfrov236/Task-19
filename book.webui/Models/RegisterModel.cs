using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace book.webui.Models
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Passwords { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Passwords")]
        public string? RePasswords { get; set; }
        [Required]
        public string? Email { get; set; }

    }
}