using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DbAccess.Data.POCO.JoiningEntity;

namespace DbAccess.Data.POCO
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(320)]
        public string EmailAddress { get; set; }

        [Required]
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
