﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.DTOs.Post;
using DBAccess.Data.POCO;
using DBAccess.Data.POCO.JoiningEntity;
using DBAccess.Repositories.Category;
using DBAccess.Repositories.Post;
using DBAccess.Repositories.Tag;
using DBAccess.Repositories.UnitOfWork;
using DBAccess.Repositories.User;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public PostService(IPostRepository repository, IMapper mapper, IUnitOfWork unitOfWork, IUserRepository userRepository, 
            ICategoryRepository categoryService, ITagRepository tagRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _categoryRepository = categoryService;
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<GetPostDto>> GetAllPosts()
        {
            return (await _repository.GetAllAsync()).Select(x =>
            {
                var postDto =  _mapper.Map<GetPostDto>(x);
                postDto.Tags = x.PostTags.Select(y => y.TagId);
                return postDto;
            }).ToList();
        }

        public async Task<IEnumerable<GetPostDto>> GetPosts(FilterSpecification<Post> filterSpecification = null, 
            PagingSpecification pagingSpecification = null,
            SortSpecification<Post> sortSpecification = null)
        {
            return (await _repository.GetAsync(filterSpecification, pagingSpecification, sortSpecification)).Select(x => _mapper.Map<GetPostDto>(x));
        }

        public async Task<int> CountPostsWhere(FilterSpecification<Post> filterSpecification = null)
        {
            return await _repository.CountWhereAsync(filterSpecification);
        }

        public async Task<IEnumerable<GetPostDto>> GetPostsFromUser(int id)
        {
            var posts = await _repository.GetPostsFromUser(id);
            return posts.Select(x =>
            {
                var postDto = _mapper.Map<GetPostDto>(x);
                postDto.Tags = x.PostTags.Select(y => y.TagId);
                return postDto;
            }).ToList();
        }

        public async Task<IEnumerable<GetPostDto>> GetPostsFromTag(int id)
        {
            return (await _repository.GetPostsFromTag(id)).Select(x =>
            {
                var postDto = _mapper.Map<GetPostDto>(x);
                postDto.Tags = x.PostTags.Select(y => y.TagId);
                return postDto;
            }).ToList();
        }

        public async Task<IEnumerable<GetPostDto>> GetPostsFromCategory(int id)
        {
            return (await _repository.GetPostsFromCategory(id)).Select(x =>
            {
                var postDto = _mapper.Map<GetPostDto>(x);
                postDto.Tags = x.PostTags.Select(y => y.TagId);
                return postDto;
            }).ToList();
        }

        public async Task<GetPostDto> GetPost(int id)
        {
            var post = await _repository.GetAsync(id);
            var postDto = _mapper.Map<GetPostDto>(post);
            postDto.Tags = post.PostTags.Select(x => x.TagId);
            return postDto;
        }

        private async Task<bool> PostAlreadyExistsWithSameProperties(UpdatePostDto post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));
            var postDb = await _repository.GetAsync(post.Id);
            if (postDb.PostTags != null && post.Tags != null &&
                !postDb.PostTags.Select(x => x.Tag.Id).SequenceEqual(post.Tags))
                return false;
            return postDb.Name == post.Name &&
                   postDb.Author.Id == post.Author &&
                   postDb.Category.Id == post.Category &&
                   postDb.Content == post.Content;
        }

        public async Task CheckPostValidity(IPostDto post)
        {
            if (post == null)
                throw new ArgumentNullException(nameof(post));
            if (string.IsNullOrWhiteSpace(post.Content))
                throw new ArgumentException("Content cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(post.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (post.Name.Length > 250)
                throw new ArgumentException("Name cannot exceed 250 characters.");
            if (await _userRepository.GetAsync(post.Author) == null)
                throw new IndexOutOfRangeException("Author doesn't exist.");
            if (await _categoryRepository.GetAsync(post.Category) == null)
                throw new IndexOutOfRangeException("Category doesn't exist.");
            post.Tags?.ToList().ForEach(x =>
            {
                var tag = _tagRepository.Get(x);
                if (tag == null)
                    throw new IndexOutOfRangeException("Tag id " + x + " doesn't exist.");
            });
            // TODO do it on each service (check duplicate) and add unitTests
            if (post.Tags != null && post.Tags.GroupBy(x => x).Any(y => y.Count() > 1))
                throw new InvalidOperationException("There can't be duplicate tags.");
        }

        public async Task CheckPostValidity(AddPostDto post)
        {
            await CheckPostValidity((IPostDto)post);
            if (await _repository.NameAlreadyExists(post.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task CheckPostValidity(UpdatePostDto post)
        {
            await CheckPostValidity((IPostDto)post);
            if (await _repository.NameAlreadyExists(post.Name) &&
                (await _repository.GetAsync(post.Id)).Name != post.Name)
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task<GetPostDto> AddPost(AddPostDto post)
        {
            await CheckPostValidity(post);
            var pocoPost = _mapper.Map<Post>(post);
            if (post.Tags != null)
                pocoPost.PostTags = post.Tags.Select(x => new PostTag()
                {
                    PostId = pocoPost.Id, 
                    TagId = x
                }).ToList();

            var result = await _repository.AddAsync(pocoPost);
            _unitOfWork.Save();
            return _mapper.Map<GetPostDto>(result);
        }

        public async Task UpdatePost(UpdatePostDto post)
        {
            if (await PostAlreadyExistsWithSameProperties(post))
                return;
            await CheckPostValidity(post);
            var postEntity = await _repository.GetAsync(post.Id);
            _mapper.Map(post, postEntity);
            _unitOfWork.Save();
        }

        public async Task DeletePost(int id)
        {
            await _repository.RemoveAsync(await _repository.GetAsync(id));
            _unitOfWork.Save();
        }
    }
}
