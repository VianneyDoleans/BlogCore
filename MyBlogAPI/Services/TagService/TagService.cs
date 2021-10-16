﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Specifications;
using DbAccess.Specifications.FilterSpecifications;
using DbAccess.Specifications.SortSpecification;
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
            return (await _repository.GetAllAsync()).Select(x => _mapper.Map<Tag, GetTagDto>(x)).ToList();
        }

        public async Task<IEnumerable<GetTagDto>> GetTags(FilterSpecification<Tag> filter = null, PagingSpecification paging = null,
            SortSpecification<Tag> sort = null)
        {
            return (await _repository.GetAsync(filter, paging, sort)).Select(x => _mapper.Map<GetTagDto>(x));
        }

        public async Task<int> CountTagsWhere(FilterSpecification<Tag> filterSpecification = null)
        {
            return await _repository.CountWhereAsync(filterSpecification);
        }

        public async Task<GetTagDto> GetTag(int id)
        {
            return _mapper.Map<GetTagDto>(await _repository.GetAsync(id));
        }

        public void CheckTagValidity(ITagDto tag)
        {
            if (tag == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(tag.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (tag.Name.Length > 50)
                throw new ArgumentException("Name cannot exceed 50 characters.");
        }

        public async Task CheckTagValidity(AddTagDto tag)
        {
            CheckTagValidity((ITagDto)tag);
            if (await _repository.NameAlreadyExists(tag.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task CheckTagValidity(UpdateTagDto tag)
        {
            CheckTagValidity((ITagDto) tag);
            if (await _repository.NameAlreadyExists(tag.Name) &&
                (await _repository.GetAsync(tag.Id)).Name != tag.Name)
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task<GetTagDto> AddTag(AddTagDto tag)
        {
            await CheckTagValidity(tag);
            var result = await _repository.AddAsync(_mapper.Map<Tag>(tag));
            _unitOfWork.Save();
            return _mapper.Map<GetTagDto>(result);
        }

        public async Task UpdateTag(UpdateTagDto tag)
        {
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
            if (tag == null)
                throw new ArgumentNullException();
            var tagDb = await _repository.GetAsync(tag.Id);
            return tagDb.Name == tag.Name;
        }
    }
}
