using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.DTOs.Comment;
using DBAccess.Data;
using DBAccess.Repositories.Comment;
using DBAccess.Repositories.Post;
using DBAccess.Repositories.UnitOfWork;
using DBAccess.Repositories.User;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;
using FluentValidation;

namespace BlogCoreAPI.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostRepository _postRepository;
        private readonly IValidator<ICommentDto> _dtoValidator;

        public CommentService(ICommentRepository repository, IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository,
            IPostRepository postRepository, IValidator<ICommentDto> dtoValidator)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _dtoValidator = dtoValidator;
        }

        public async Task<IEnumerable<GetCommentDto>> GetAllComments()
        {
            return (await _repository.GetAllAsync()).Select(x => _mapper.Map<GetCommentDto>(x)).ToList();
        }

        public async Task<IEnumerable<GetCommentDto>> GetComments(FilterSpecification<Comment> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Comment> sortSpecification = null)
        {
            return (await _repository.GetAsync(filterSpecification, pagingSpecification, sortSpecification)).Select(x => _mapper.Map<GetCommentDto>(x));
        }

        public async Task<int> CountCommentsWhere(FilterSpecification<Comment> filterSpecification = null)
        {
            return await _repository.CountWhereAsync(filterSpecification);
        }

        public async Task<GetCommentDto> GetComment(int id)
        {
            var comment = await _repository.GetAsync(id);
                return _mapper.Map<GetCommentDto>(comment);
        }

        private async Task<bool> CommentAlreadyExistsWithSameProperties(UpdateCommentDto comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));
            var commentDb = await _repository.GetAsync(comment.Id);
            return commentDb.Content == comment.Content &&
                   commentDb.Author.Id == comment.Author &&
                   commentDb.CommentParent?.Id == comment.CommentParent &&
                   commentDb.PostParent?.Id == comment.PostParent;
        }

        public async Task CheckCommentValidity(ICommentDto comment)
        {
            if (await _userRepository.GetAsync(comment.Author) == null)
                throw new IndexOutOfRangeException("Author doesn't exist.");
            if (await _postRepository.GetAsync(comment.PostParent) == null)
                throw new IndexOutOfRangeException("Post parent doesn't exist.");
            if (comment.CommentParent != null && await _repository.GetAsync(comment.CommentParent.Value) == null)
                throw new IndexOutOfRangeException("Comment parent doesn't exist.");
        }

        public async Task CheckCommentValidity(UpdateCommentDto comment)
        {
            await CheckCommentValidity((ICommentDto)comment);
            if (comment.CommentParent != null)
            {
                var commentParent = await _repository.GetAsync(comment.CommentParent.Value);
                if (commentParent.Id == comment.Id)
                    throw new InvalidOperationException("Comment's comment parent cannot be itself.");
            }
        }

        public async Task<GetCommentDto> AddComment(AddCommentDto comment)
        {
            await _dtoValidator.ValidateAndThrowAsync(comment);
            await CheckCommentValidity(comment);
            var result = await _repository.AddAsync(_mapper.Map<Comment>(comment));
            _unitOfWork.Save();
            return _mapper.Map<GetCommentDto>(result);
        }

        public async Task UpdateComment(UpdateCommentDto comment)
        {
            await _dtoValidator.ValidateAndThrowAsync(comment);
            if (await CommentAlreadyExistsWithSameProperties(comment))
                return;
            await CheckCommentValidity(comment);
            var commentEntity = await _repository.GetAsync(comment.Id);
            _mapper.Map(comment, commentEntity);
            _unitOfWork.Save();
        }

        public async Task DeleteComment(int id)
        {
            await _repository.RemoveAsync(await _repository.GetAsync(id));
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
