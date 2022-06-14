﻿using System.Collections.Generic;
using DBAccess.Data;

namespace BlogCoreAPI.DTOs.Category
{
    /// <summary>
    /// GET Dto type of <see cref="Category"/>.
    /// </summary>
    public class GetCategoryDto : ADto, ICategoryDto
    {
        public string Name { get; set; }

        public virtual IEnumerable<int> Posts { get; set; }
    }
}
