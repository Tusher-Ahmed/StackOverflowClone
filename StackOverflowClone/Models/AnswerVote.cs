using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflowClone.Models
{
    public class AnswerVote
    {
        public virtual long Id { get; set; }
        public virtual bool PositiveVote { get; set; }
        public virtual bool NegativeVote { get; set; }
        public virtual long QuestionId { get; set; }
        public virtual long AnswerId { get; set; }
        public virtual long ClientId { get; set; }
    }
}