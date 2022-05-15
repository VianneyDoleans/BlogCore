using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Like;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Repositories.User;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
using MyBlogAPI.DTOs.Like;

namespace MyBlogAPI.Services.LikeService
{
    public class LikeService : ILikeService
    {
        private readonly ILikeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;

        public LikeService(ILikeRepository repository, IMapper mapper, IUnitOfWork unitOfWork,
            ICommentRepository commentRepository, IPostRepository postRepository, IUserRepository userRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<GetLikeDto>> GetAllLikes()
        {
            return (await _repository.GetAllAsync()).Select(x => _mapper.Map<GetLikeDto>(x)).ToList();
        }

        public async Task<IEnumerable<GetLikeDto>> GetLikes(FilterSpecification<Like> filter = null, 
            PagingSpecification paging = null,
            SortSpecification<Like> sort = null)
        {
            return (await _repository.GetAsync(filter, paging, sort)).Select(x => _mapper.Map<GetLikeDto>(x));
        }

        public async Task<int> CountLikesWhere(FilterSpecification<Like> filterSpecification = null)
        {
            return await _repository.CountWhereAsync(filterSpecification);
        }

        public async Task<GetLikeDto> GetLike(int id)
        {
            return _mapper.Map<GetLikeDto>(await _repository.GetAsync(id));
        }

        public async Task CheckLikeValidity(ILikeDto like)
        {
            // TODO maybe remove LikeableType (not so much useful)
            if (like == null)
                throw new ArgumentNullException(nameof(like));
            if (await _userRepository.GetAsync(like.User) == null)
                throw new IndexOutOfRangeException("User doesn't exist.");
            if (like.Comment != null && like.Post != null)
                throw new InvalidOperationException("A like can't be assigned to a comment and a post at the same time.");
            switch (like.LikeableType)
            {
                case LikeableType.Comment when like.Comment == null:
                    throw new ArgumentException("Comment cannot be null.");
                case LikeableType.Comment when await _commentRepository.GetAsync(like.Comment.Value) == null:
                    throw new IndexOutOfRangeException("Comment doesn't exist.");
                case LikeableType.Post when like.Post == null:
                    throw new ArgumentException("Post cannot be null.");
                case LikeableType.Post when await _postRepository.GetAsync(like.Post.Value) == null:
                    throw new IndexOutOfRangeException("Post doesn't exist.");
            }
        }

        public async Task CheckLikeValidity(AddLikeDto like)
        {
            await CheckLikeValidity((ILikeDto)like);
            if (await _repository.LikeAlreadyExists(_mapper.Map<Like>(like)))
                throw new InvalidOperationException("Like already exists.");
        }

        public async Task<GetLikeDto> AddLike(AddLikeDto like)
        {
            await CheckLikeValidity(like);
            var result = await _repository.AddAsync(_mapper.Map<Like>(like));
            _unitOfWork.Save();
            return _mapper.Map<GetLikeDto>(result);
        }

        public async Task UpdateLike(UpdateLikeDto like)
        {
            if (await LikeAlreadyExistsWithSameProperties(like))
                return;
            await CheckLikeValidity(like);
            var likeEntity = await _repository.GetAsync(like.Id);
            _mapper.Map(like, likeEntity);
            _unitOfWork.Save();
        }

        public async Task DeleteLike(int id)
        {
            await _repository.RemoveAsync(await _repository.GetAsync(id));
            _unitOfWork.Save();
        }

        private async Task<bool> LikeAlreadyExistsWithSameProperties(UpdateLikeDto like)
        {
            // TODO maybe remove LikeableType (not so much useful)
            if (like == null)
                throw new ArgumentNullException(nameof(like));
            var likeDb = await _repository.GetAsync(like.Id);
            // TODO check ?.Id in every "if comparison" conditions (null) (only if property have the right to be null)
            return likeDb.Comment?.Id == like.Comment &&
                   likeDb.LikeableType == like.LikeableType &&
                   likeDb.Post?.Id == like.Post &&
                   likeDb.User.Id == like.User;
        }

        public async Task<IEnumerable<GetLikeDto>> GetLikesFromUser(int id)
        {
            return (await _repository.GetLikesFromUser(id)).Select(x => _mapper.Map<GetLikeDto>(x)).ToList();
        }

        public async Task<IEnumerable<GetLikeDto>> GetLikesFromPost(int id)
        {
            return (await _repository.GetLikesFromPost(id)).Select(x => _mapper.Map<GetLikeDto>(x)).ToList();
        }

        public async Task<IEnumerable<GetLikeDto>> GetLikesFromComment(int id)
        {
            return (await _repository.GetLikesFromComment(id)).Select(x => _mapper.Map<GetLikeDto>(x)).ToList();
        }
    }
}
