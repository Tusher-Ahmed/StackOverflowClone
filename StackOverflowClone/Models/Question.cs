﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace StackOverflowClone.Models
{
    public class Question
    {
        public virtual long Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Body { get; set; }
        public virtual string ExpectResult { get; set; }
        public virtual string Tags { get; set; }
        public virtual long ClientId { get; set; }
        public virtual Client Client { get; set; }      
        public virtual DateTime CreatedAt { get; set; }
        
    }
}