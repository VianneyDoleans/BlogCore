using System;
using System.Collections.Generic;
using System.Text;
using DbAccess.Data.Models;

namespace DbAccess.Data.JoiningEntity
{
    public class PostTag
    {
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}   
