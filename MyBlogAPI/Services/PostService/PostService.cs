﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Data.POCO.JoiningEntity;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.Tag;
using DbAccess.Repositories.UnitOfWork;
using DbAccess.Repositories.User;
using MyBlogAPI.DTO.Post;
using MyBlogAPI.Services.UserService;

namespace MyBlogAPI.Services.PostService
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
            return _repository.GetAll().Select(x =>
            {
                var postDto =  _mapper.Map<GetPostDto>(x);
                postDto.Tags = x.PostTags.Select(y => y.TagId);
                return postDto;
            }).ToList();
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
            var post = await GetPostFromRepository(id);
            var postDto = _mapper.Map<GetPostDto>(post);
            postDto.Tags = post.PostTags.Select(x => x.TagId);
            return postDto;
        }

        public async Task CheckPostValidity(AddPostDto post)
        {
            if (post == null)
                throw new ArgumentNullException();
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
            if (await _repository.NameAlreadyExists(post.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task CheckPostValidity(UpdatePostDto post)
        {
            if (post == null)
                throw new ArgumentNullException();
            var postDb = await GetPostFromRepository(post.Id);
            if (postDb.Name == post.Name &&
                postDb.Author.Id == post.Author &&
                postDb.Category.Id == post.Category &&
                postDb.Content == post.Content &&
                postDb.PostTags.Select(x => x.Tag.Id).SequenceEqual(post.Tags))
                return;
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
            // TODO REMAKE THIS LINE
            if (await _repository.NameAlreadyExists(post.Name))
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

        private async Task<Post> GetPostFromRepository(int id)
        {
            try
            {
                var postDb = await _repository.GetAsync(id);
                if (postDb == null)
                    throw new IndexOutOfRangeException("Post doesn't exist.");
                return postDb;
            }
            catch
            {
                throw new IndexOutOfRangeException("Post doesn't exist.");
            }
        }

        public async Task UpdatePost(UpdatePostDto post)
        {
            // TODO
            await CheckPostValidity(post);
            var postEntity = await GetPostFromRepository(post.Id);
            postEntity.Category.Id = post.Category;
            postEntity.Name = post.Name;
        }

        public async Task DeletePost(int id)
        {
            await _repository.RemoveAsync(await GetPostFromRepository(id));
            _unitOfWork.Save();
        }
    }
}
