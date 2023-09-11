using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflowClone.Models
{
    public class QuestionVote
    {
        public virtual long Id { get; set; }
        public virtual bool PosVote { get; set; }
        public virtual bool NegVote { get; set; }
        public virtual long ClientId { get; set; }
        public virtual long QuestionId { get; set; }
    }
}