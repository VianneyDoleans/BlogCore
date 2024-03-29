﻿using System;
using System.Collections.Generic;

namespace BlogCoreAPI.Models.DTOs.Account
{
    /// <summary>
    /// GET Dto type of <see cref="Account"/>.
    /// </summary>
    public class GetAccountDto : ADto
    {
        public string UserName { get; set; }

        public string Email { get; set; }
        
        public string ProfilePictureUrl { get; set; }

        public DateTimeOffset RegisteredAt { get; set; }

        public DateTimeOffset LastLogin { get; set; }

        public string UserDescription { get; set; }

        public virtual IEnumerable<int> Roles { get; set; }
    }
}
