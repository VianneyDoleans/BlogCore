using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Like;
using DbAccess.Repositories.UnitOfWork;
using MyBlogAPI.DTO;
using MyBlogAPI.DTO.Like;

namespace MyBlogAPI.Services.LikeService
{
    public class LikeService : ILikeService
    {

        private readonly ILikeRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public LikeService(ILikeRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<GetLikeDto>> GetAllLikes()
        {
            return _repository.GetAll().Select(x => _mapper.Map<GetLikeDto>(x)).ToList();
        }

        public async Task<GetLikeDto> GetLike(int id)
        {
            return _mapper.Map<GetLikeDto>(_repository.Get(id));
        }

        public async Task<GetLikeDto> AddLike(AddLikeDto like)
        {
            var result = _repository.Add(_mapper.Map<Like>(like));
            _unitOfWork.Save();
            return _mapper.Map<GetLikeDto>(result);
        }

        public async Task UpdateLike(UpdateLikeDto like)
        {
            var userEntity = _repository.Get(like.Id);
            //TODO
            _unitOfWork.Save();
        }

        public async Task DeleteLike(int id)
        {
            _repository.Remove(_repository.Get(id));
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
