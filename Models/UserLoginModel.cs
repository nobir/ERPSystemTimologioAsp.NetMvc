using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ERPSystemTimologio.EF;

namespace ERPSystemTimologio.Models
{
    public class UserLoginModel: User
    {
        public UserLoginModel()
        {

        }

        [Required, EmailAddress]
        public new string Email { get; set; }
        [Required]
        public new string Password { get; set; }
    }
}