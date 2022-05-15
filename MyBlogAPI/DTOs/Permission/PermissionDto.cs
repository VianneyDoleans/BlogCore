namespace MyBlogAPI.DTOs.Permission
{
    /// <summary>
    /// GET Dto type of <see cref="DbAccess.Data.POCO.Permission.Permission"/>
    /// </summary>
    public class PermissionDto
    {
        public PermissionActionDto PermissionAction { get; set; }

        public PermissionTargetDto PermissionTarget { get; set; }

        public PermissionRangeDto PermissionRange { get; set; }
    }
}
