namespace BlogCoreAPI.DTOs.Permission
{
    /// <summary>
    /// GET Dto type of <see cref="Permission"/>
    /// </summary>
    public class PermissionDto
    {
        public PermissionActionDto PermissionAction { get; set; }

        public PermissionTargetDto PermissionTarget { get; set; }

        public PermissionRangeDto PermissionRange { get; set; }
    }
}
