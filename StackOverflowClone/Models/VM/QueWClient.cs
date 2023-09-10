using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StackOverflowClone.Models.VM
{
    public class QueWClient
    {
        public Question Question { get; set; }
        public IEnumerable<Question> Questions { get; set; }
        public IEnumerable<Answer> Answers { get; set; }
        public Answer Answer { get; set; }
        public Client Client { get; set; }
        public Dictionary<long, string> MapedQC { get; set; }
    }
}