using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace StackOverflowClone.Models
{
    public class Question
    {
        [Key]
        public virtual long Id { get; set; }
        [Required]
        public virtual string Title { get; set; }
        [Required]
        public virtual string Body { get; set; }
        [Required]
        public virtual string ExpectResult { get; set; }
        [Required]
        public virtual string Tags { get; set; }
        public virtual long ClientId { get; set; }    
        public virtual long QVote { get; set; }      
        public virtual long Answer { get; set; }    
        public virtual long Views { get; set; }    
        public virtual DateTime CreatedAt { get; set; }
        public virtual bool Approve { get; set; }
        
    }
}