using System;
using System.Collections.Generic;
using System.Text;

namespace DbAccess.Data.POCO.Interface
{
    public interface IHasCommentParent
    {
        public Comment CommentParent { get; set; }
    }
}
