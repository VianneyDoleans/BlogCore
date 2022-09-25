using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.Models.DTOs.Tag;
using BlogCoreAPI.Models.Exceptions;
using DBAccess.Data;
using DBAccess.Repositories.Tag;
using DBAccess.Repositories.UnitOfWork;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;
using FluentValidation;

namespace BlogCoreAPI.Services.TagService
{
    public class TagService : ITagService
    {

        private readonly ITagRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<ITagDto> _dtoValidator;

        public TagService(ITagRepository repository, IMapper mapper, IUnitOfWork unitOfWork, IValidator<ITagDto> dtoValidator)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _dtoValidator = dtoValidator;
        }

        public async Task<IEnumerable<GetTagDto>> GetAllTags()
        {
            return (await _repository.GetAllAsync()).Select(x => _mapper.Map<Tag, GetTagDto>(x)).ToList();
        }

        public async Task<IEnumerable<GetTagDto>> GetTags(FilterSpecification<Tag> filterSpecification = null, PagingSpecification pagingSpecification = null,
            SortSpecification<Tag> sortSpecification = null)
        {
            return (await _repository.GetAsync(filterSpecification, pagingSpecification, sortSpecification)).Select(x => _mapper.Map<GetTagDto>(x));
        }

        public async Task<int> CountTagsWhere(FilterSpecification<Tag> filterSpecification = null)
        {
            return await _repository.CountWhereAsync(filterSpecification);
        }

        public async Task<GetTagDto> GetTag(int id)
        {
            return _mapper.Map<GetTagDto>(await _repository.GetAsync(id));
        }

        public async Task CheckTagValidity(AddTagDto tag)
        {
            if (await _repository.NameAlreadyExists(tag.Name))
                throw new InvalidRequestException("Name already exists.");
        }

        public async Task CheckTagValidity(UpdateTagDto tag)
        {
            if (await _repository.NameAlreadyExists(tag.Name) &&
                (await _repository.GetAsync(tag.Id)).Name != tag.Name)
                throw new InvalidRequestException("Name already exists.");
        }

        public async Task<GetTagDto> AddTag(AddTagDto tag)
        {
            await _dtoValidator.ValidateAndThrowAsync(tag);
            await CheckTagValidity(tag);
            var result = await _repository.AddAsync(_mapper.Map<Tag>(tag));
            _unitOfWork.Save();
            return _mapper.Map<GetTagDto>(result);
        }

        public async Task UpdateTag(UpdateTagDto tag)
        {
            await _dtoValidator.ValidateAndThrowAsync(tag);
            if (await TagAlreadyExistsWithSameProperties(tag))
                return;
            await CheckTagValidity(tag);
            var tagEntity = await _repository.GetAsync(tag.Id);
            tagEntity.Name = tag.Name;
            _unitOfWork.Save();
        }

        public async Task DeleteTag(int id)
        {
            await _repository.RemoveAsync(await _repository.GetAsync(id));
            _unitOfWork.Save();
        }

        private async Task<bool> TagAlreadyExistsWithSameProperties(UpdateTagDto tag)
        {
            var tagDb = await _repository.GetAsync(tag.Id);
            return tagDb.Name == tag.Name;
        }
    }
}
