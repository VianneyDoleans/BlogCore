using BlogCoreAPI.Models.DTOs.Contracts;
using DBAccess.Data;

namespace BlogCoreAPI.Models.DTOs.Like
{
    /// <summary>
    /// Interface of <see cref="Like"/> Dto containing all the common properties of Like Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface ILikeDto : IHasUser
    {
        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public new int User { get; set; }
    }
}
