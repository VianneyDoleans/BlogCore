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

        public async Task CheckTagValidity(AddTagDto tag)
        {
            if (tag == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(tag.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (tag.Name.Length > 50)
                throw new ArgumentException("Name cannot exceed 50 characters.");
            if (await _repository.NameAlreadyExists(tag.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task CheckTagValidity(UpdateTagDto tag)
        {
            if (tag == null)
                throw new ArgumentNullException();
            if (_repository.GetAsync(tag.Id) == null)
                throw new ArgumentException("Tag doesn't exist.");
            if (string.IsNullOrWhiteSpace(tag.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (tag.Name.Length > 50)
                throw new ArgumentException("Name cannot exceed 50 characters.");
            if (await _repository.NameAlreadyExists(tag.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task<GetTagDto> AddTag(AddTagDto tag)
        {
            await CheckTagValidity(tag);
            var result = _repository.Add(_mapper.Map<Tag>(tag));
            _unitOfWork.Save();
            return _mapper.Map<GetTagDto>(result);
        }

        public async Task UpdateTag(UpdateTagDto tag)
        {
            await CheckTagValidity(tag);
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
