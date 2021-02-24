﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using MyBlogAPI.DTO.Category;
using MyBlogAPI.DTO.Comment;
using MyBlogAPI.DTO.Like;
using MyBlogAPI.DTO.Tag;
using MyBlogAPI.DTO.User;

namespace MyBlogAPI.DTO.Post
{
    public class AddPostDto
    {
        public string Content { get; set; }

        public string Name { get; set; }

        public int Author { get; set; }

        public int Category { get; set; }

        public virtual IEnumerable<int> Tags { get; set; }
    }
}
