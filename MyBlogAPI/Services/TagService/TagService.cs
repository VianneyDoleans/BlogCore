using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Repositories.User;
using MyBlogAPI.DTO.Tag;

namespace MyBlogAPI.Services.TagService
{
    public class TagService : ITagService
    {

        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TagService(ITagRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetTagDto>> GetAllTags()
        {
            return _repository.GetAll().Select(x => _mapper.Map<Tag, GetTagDto>(x)).ToList();
        }

        public async Task<GetTagDto> GetTag(int id)
        {
            /*var userEntity = _repository.Get(id);
            var userDto = _mapper.Map<GetTagDto>(userEntity);
            userDto.Posts = userEntity.PostTags.Where(x => x.TagId == id).Select(x => x.PostId);*/
            return _mapper.Map<GetTagDto>(_repository.Get(id));
        }

        public async Task AddTag(AddTagDto tag)
        {
            _repository.Add(_mapper.Map<Tag>(tag));
            _unitOfWork.Save();
        }

        public async Task UpdateTag(AddTagDto tag)
        {
            var tagEntity = _repository.Get(tag.Id);
            tagEntity.Name = tag.Name;
            _unitOfWork.Save();
        }

        public async Task DeleteTag(int id)
        {
            _repository.Remove(_repository.Get(id));
            _unitOfWork.Save();
        }
    }
}
