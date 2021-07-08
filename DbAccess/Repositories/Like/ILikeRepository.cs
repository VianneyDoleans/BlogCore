﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAccess.Specifications;

namespace DbAccess.Repositories.Like
{
    public interface ILikeRepository : IRepository<Data.POCO.Like>
    {
        Task<IEnumerable<Data.POCO.Like>> GetLikesFromPost(int id);

        Task<IEnumerable<Data.POCO.Like>> GetLikesFromUser(int id);

        Task<IEnumerable<Data.POCO.Like>> GetLikesFromComment(int id);

        Task<bool> LikeAlreadyExists(Data.POCO.Like like);
    }
}
