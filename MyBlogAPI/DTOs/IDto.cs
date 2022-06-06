namespace MyBlogAPI.DTOs
{
    /// <summary>
    /// Interface of all Dto object resource which include the mandatory properties needed to classify an object as a Dto
    /// </summary>
    public interface IDto
    {
        /// <summary>
        /// Id of the resource. This element is necessary to recover the object inside Database.
        /// </summary>
        int Id { get; set; }
    }
}
