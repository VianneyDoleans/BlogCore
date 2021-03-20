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
using MyBlogAPI.DTO.Like;

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
            return _repository.GetAll().Select(x => _mapper.Map<GetLikeDto>(x)).ToList();
        }

        public async Task<GetLikeDto> GetLike(int id)
        {
            return _mapper.Map<GetLikeDto>(await GetLikeFromRepository(id));
        }

        public async Task CheckLikeValidity(AddLikeDto like)
        {
            // TODO maybe remove LikeableType (not so much useful)
            if (like == null)
                throw new ArgumentNullException();
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
            if (await _repository.LikeAlreadyExists(_mapper.Map<Like>(like)))
                throw new InvalidOperationException("Like already exists.");
        }

        public async Task CheckLikeValidity(UpdateLikeDto like)
        {
            // TODO maybe remove LikeableType (not so much useful)
            if (like == null)
                throw new ArgumentNullException();
            var likeDb = await GetLikeFromRepository(like.Id);
            // TODO check ?.Id in every "if comparison" conditions (null) (only if propery have the right to be null)
            if (likeDb.Comment?.Id == like.Comment && 
                likeDb.LikeableType == like.LikeableType &&
                likeDb.Post?.Id == like.Post &&
                likeDb.User.Id == like.User)
                return;
            if (await _userRepository.GetAsync(like.User) == null)
                throw new IndexOutOfRangeException("User doesn't exist.");
            if (like.Comment != null && like.Post != null)
                throw new ArgumentException("A like can't be assigned to a comment and a post at the same time.");
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
                default:
                    return;
            }
        }

        private async Task<Like> GetLikeFromRepository(int id)
        {
            try
            {
                var likeDb = await _repository.GetAsync(id);
                if (likeDb == null)
                    throw new IndexOutOfRangeException("Like doesn't exist.");
                return likeDb;
            }
            catch
            {
                throw new IndexOutOfRangeException("Like doesn't exist.");
            }
        }

        public async Task<GetLikeDto> AddLike(AddLikeDto like)
        {
            await CheckLikeValidity(like);
            var result = _repository.Add(_mapper.Map<Like>(like));
            _unitOfWork.Save();
            return _mapper.Map<GetLikeDto>(result);
        }

        public async Task UpdateLike(UpdateLikeDto like)
        {
            await CheckLikeValidity(like);
            var likeEntity = await GetLikeFromRepository(like.Id);
            _mapper.Map(like, likeEntity);
            _unitOfWork.Save();
        }

        public async Task DeleteLike(int id)
        {
            _repository.Remove(await GetLikeFromRepository(id));
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<GetLikeDto>> GetLikesFromUser(int id)
        {
            //return (await _repository.GetWhereAsync(x => x.User.Id == id)).Select(x => _mapper.Map<GetLikeDto>(x)).ToList();
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
