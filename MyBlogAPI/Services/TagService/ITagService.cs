using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlogAPI.DTO;

namespace MyBlogAPI.Services.TagService
{
    public interface ITagService
    {
        ICollection<Tag> GetAllTags();

        Tag GetTag(int id);

        void AddTag(Tag user);

        void UpdateTag(Tag user);

        void DeleteTag(int id);
    }
}
