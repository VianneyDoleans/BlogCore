using DbAccess.Data.POCO;

namespace MyBlogAPI.DTO.Like
{
    /// <summary>
    /// Interface of <see cref="DbAccess.Data.POCO.Like"/> Dto containing all the common properties of Like Dto Type (GET, ADD, UPDATE).
    /// </summary>
    public interface ILikeDto
    {
        public LikeableType LikeableType { get; set; }

        public int? Comment { get; set; }

        public int? Post { get; set; }

        public int User { get; set; }
    }
}
