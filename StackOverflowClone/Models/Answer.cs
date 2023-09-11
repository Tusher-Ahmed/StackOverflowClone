using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflowClone.Models
{
    public class Answer
    {
        public virtual long Id { get; set; }
        public virtual string QuestionsAnswer { get; set; }
        public virtual string Username { get; set; }
        public virtual long QuestionId { get; set; }
        public virtual long Vote { get; set; }
        public virtual DateTime CreatedAt { get; set; }
    }
}