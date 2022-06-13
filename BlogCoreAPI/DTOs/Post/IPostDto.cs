using System.Collections.Generic;
using BlogCoreAPI.DTOs.Contracts;
using DBAccess.Data;

namespace BlogCoreAPI.DTOs.Post
{
    /// <summary>
    /// Interface of <see cref="Post"/> Dto containing all the common properties of Post Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface IPostDto : IHasAuthor
    {
        public string Content { get; set; }

        public string Name { get; set; }

        public new int Author { get; set; }

        public int Category { get; set; }

        public IEnumerable<int> Tags { get; set; }
    }
}
