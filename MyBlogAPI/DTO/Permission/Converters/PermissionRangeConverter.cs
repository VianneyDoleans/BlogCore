﻿using AutoMapper;
using DbAccess.Data.POCO.Permission;

namespace MyBlogAPI.DTO.Permission.Converters
{
    /// <summary>
    /// AutoMapper converter used to enable the conversion of <see cref="PermissionRange"/> to <see cref="PermissionRangeDto"/>.
    /// </summary>
    public class PermissionRangeConverter : ITypeConverter<PermissionRange, PermissionRangeDto>
    {
        /// <inheritdoc />
        public PermissionRangeDto Convert(PermissionRange source, PermissionRangeDto destination,
            ResolutionContext context)
        {
            destination.Name = source.ToString();
            destination.Id = (int)source;
            return destination;
        }
    }
}
