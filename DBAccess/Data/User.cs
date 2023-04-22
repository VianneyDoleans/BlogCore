using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DBAccess.Contracts;
using DBAccess.Data.JoiningEntity;
using Microsoft.AspNetCore.Identity;

namespace DBAccess.Data
{
    public class User : IdentityUser<int>, IPoco, IHasUserName, IHasEmail, IHasRegisteredAt, IHasLastLogin, IHasUserRoles, IHasPosts, IHasComments, IHasLikes
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public override string UserName { get; set; }
        
        public string ProfilePictureUrl { get; set; }
        
        [Required]
        [MaxLength(320)]
        public override string Email { get; set; }

        [Required]
        [NotMapped]
        public string Password { get; set; }

        [Required]
        public DateTime RegisteredAt { get; set; }

        [Required]
        public DateTime LastLogin { get; set; }

        [MaxLength(1000)]
        public string UserDescription { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<UserRole> UserRoles { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<Post> Posts { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<Comment> Comments { get; set; }

        [ForeignKey("UserId")]
        public virtual ICollection<Like> Likes { get; set; }
    }
}
