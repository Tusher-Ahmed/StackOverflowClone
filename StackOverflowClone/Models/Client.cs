using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflowClone.Models
{
    public class Client
    {

        public virtual long Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string AboutMe { get; set; }
        public virtual string Password { get; set; }
    
    }
}