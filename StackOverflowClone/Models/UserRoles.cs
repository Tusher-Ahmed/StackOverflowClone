using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflowClone.Models
{
    public class UserRoles
    {
        public virtual long Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Role { get; set; }
    }
}