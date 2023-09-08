using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StackOverflowClone.Models
{
    public class Client
    {

        public virtual long Id { get; set; }
        public virtual string Username { get; set; }
        [Required]
        public virtual string Email { get; set; }
        public virtual string AboutMe { get; set; }
        [Required]
        public virtual string Password { get; set; }
    
    }
}