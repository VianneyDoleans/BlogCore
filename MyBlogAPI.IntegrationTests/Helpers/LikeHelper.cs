using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using DbAccess.Data.POCO;
using MyBlogAPI.DTO.Like;

namespace MyBlogAPI.IntegrationTests.Helpers
{
    public class LikeHelper : AEntityHelper<GetLikeDto, AddLikeDto, UpdateLikeDto>
    {
        public LikeHelper(HttpClient client, string baseUrl = "/likes") : base(baseUrl, client)
        {
        }

        protected override AddLikeDto CreateTAdd()
        {
            var user = new AddLikeDto()
            {
                Comment = 1,
                LikeableType = LikeableType.Comment,
                User = 1
            };
            return user;
        }

        public override bool Equals(GetLikeDto first, GetLikeDto second)
        {
            if (first == null || second == null)
                return false;
            return first.User == second.User &&
                   first.Post == second.Post &&
                   first.LikeableType == second.LikeableType &&
                   first.Comment == second.Comment;
        }

        protected override UpdateLikeDto ModifyTUpdate(UpdateLikeDto entity)
        {
            return new UpdateLikeDto {Id = entity.Id, LikeableType = LikeableType.Post, Post = 1, User = entity.User};
        }
    }
}
