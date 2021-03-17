using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Repositories.User;
using MyBlogAPI.DTO.Comment;

namespace MyBlogAPI.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRepository _postRepository;

        public CommentService(ICommentRepository repository, IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository,
            IPostRepository postRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<GetCommentDto>> GetAllComments()
        {
            return (await _repository.GetAllAsync()).Select(x => _mapper.Map<GetCommentDto>(x)).ToList();
        }

        public async Task<GetCommentDto> GetComment(int id)
        {
            var comment = await GetCommentFromRepository(id);
                return _mapper.Map<GetCommentDto>(comment);
        }

        public async Task CheckCommentValidity(AddCommentDto comment)
        {
            if (comment == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(comment.Content))
                throw new ArgumentException("Content cannot be null or empty.");
            if (await _userRepository.GetAsync(comment.Author) == null)
                throw new ArgumentException("Author doesn't exist.");
            if (await _postRepository.GetAsync(comment.PostParent) == null)
                throw new ArgumentException("Post parent doesn't exist.");
            if (comment.CommentParent != null && await _repository.GetAsync(comment.CommentParent.Value) == null)
                throw new ArgumentException("Comment parent doesn't exist.");
        }

        public async Task CheckCommentValidity(UpdateCommentDto comment)
        {
            if (comment == null)
                throw new ArgumentNullException();
            var commentDb = await GetCommentFromRepository(comment.Id);
            if (commentDb.Content == comment.Content &&
                commentDb.Author.Id == comment.Author &&
                commentDb.CommentParent?.Id == comment.CommentParent &&
                commentDb.PostParent?.Id == comment.PostParent)
                return;
            if (string.IsNullOrWhiteSpace(comment.Content))
                throw new ArgumentException("Content cannot be null or empty.");
            // TODO  Add a verification for all ID properties in all services (GetItemFromRepository() throw error)
            if (await _userRepository.GetAsync(comment.Author) == null)
                throw new ArgumentException("Author doesn't exist.");
            if (await _postRepository.GetAsync(comment.PostParent) == null)
                throw new ArgumentException("Post parent doesn't exist.");
            if (comment.CommentParent != null && await _repository.GetAsync(comment.CommentParent.Value) == null)
                throw new ArgumentException("Comment parent doesn't exist.");
            if (comment.CommentParent != null)
            {
                var commentParent = await _repository.GetAsync(comment.CommentParent.Value);
                if (commentParent.Id == comment.Id)
                    throw new InvalidOperationException("Comment's comment parent cannot be itself.");
            }
        }

        private async Task<Comment> GetCommentFromRepository(int id)
        {
            try
            {
                var commentDb = await _repository.GetAsync(id);
                if (commentDb == null)
                    throw new IndexOutOfRangeException("Comment doesn't exist.");
                return commentDb;
            }
            catch
            {
                throw new IndexOutOfRangeException("Comment doesn't exist.");
            }
        }

        public async Task<GetCommentDto> AddComment(AddCommentDto comment)
        {
            await CheckCommentValidity(comment);
            var result = await _repository.AddAsync(_mapper.Map<Comment>(comment));
            _unitOfWork.Save();
            return _mapper.Map<GetCommentDto>(result);
        }

        public async Task UpdateComment(UpdateCommentDto comment)
        {
            await CheckCommentValidity(comment);
            var commentEntity = await GetCommentFromRepository(comment.Id);
            _mapper.Map(comment, commentEntity);
            _unitOfWork.Save();
        }

        public async Task DeleteComment(int id)
        {
            await _repository.RemoveAsync(await GetCommentFromRepository(id));
            _unitOfWork.Save();
        }

        public async Task<IEnumerable<GetCommentDto>> GetCommentsFromUser(int id)
        {
            return (await _repository.GetCommentsFromUser(id)).Select(x => _mapper.Map<GetCommentDto>(x)).ToList();
        }

        public async Task<IEnumerable<GetCommentDto>> GetCommentsFromPost(int id)
        {
            return (await _repository.GetCommentsFromPost(id)).Select(x => _mapper.Map<GetCommentDto>(x)).ToList();
        }
    }
}
