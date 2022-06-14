using System.Collections.Generic;
using DBAccess.Data;

namespace BlogCoreAPI.DTOs.Post
{
    /// <summary>
    /// UPDATE Dto type of <see cref="Post"/>.
    /// </summary>
    public class UpdatePostDto : ADto, IPostDto
    {
        public string Content { get; set; }

        public string Name { get; set; }

        public int Author { get; set; }

        public int Category { get; set; }

        public virtual IEnumerable<int> Tags { get; set; }
    }
}
