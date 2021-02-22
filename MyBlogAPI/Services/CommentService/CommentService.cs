using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Comment;
using DbAccess.Repositories.UnitOfWork;
using MyBlogAPI.DTO.Comment;

namespace MyBlogAPI.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(ICommentRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetCommentDto>> GetAllComments()
        {
            return _repository.GetAll().Select(x => _mapper.Map<GetCommentDto>(x)).ToList();
        }

        public async Task<GetCommentDto> GetComment(int id)
        {
            var comment = _repository.Get(id);
            return _mapper.Map<GetCommentDto>(comment);
        }

        public async Task<GetCommentDto> AddComment(AddCommentDto comment)
        {
            var result = _repository.Add(_mapper.Map<Comment>(comment));
            _unitOfWork.Save();
            return _mapper.Map<GetCommentDto>(result);
        }

        public async Task UpdateComment(UpdateCommentDto comment)
        {
            var commentEntity = _repository.Get(comment.Id);
            commentEntity.Content = comment.Content;
            //TODO
            _unitOfWork.Save();
        }

        public async Task DeleteComment(int id)
        {
            _repository.Remove(_repository.Get(id));
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
