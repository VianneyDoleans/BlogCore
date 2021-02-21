using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Data.POCO.JoiningEntity;
using DbAccess.Repositories.Post;
using DbAccess.Repositories.UnitOfWork;
using MyBlogAPI.DTO.Post;

namespace MyBlogAPI.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IPostRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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
            var post = _repository.Get(id);
            var postDto = _mapper.Map<GetPostDto>(post);
            postDto.Tags = post.PostTags.Select(x => x.TagId);
            return postDto;
        }

        public async Task<GetPostDto> AddPost(AddPostDto post)
        {
            var pocoPost = _mapper.Map<Post>(post);
            for (var i = 0; i < post.Tags.Count; i += 1)
            {
                pocoPost.PostTags.Add(new PostTag() {PostId = pocoPost.Id, TagId = post.Tags.ToArray()[i]});
            }
            //TODO
            var result = _repository.Add(pocoPost);
            _unitOfWork.Save();
            return _mapper.Map<GetPostDto>(result);
        }

        public async Task UpdatePost(UpdatePostDto post)
        {
            var postEntity = _repository.Get(post.Id);
            postEntity.Category.Id = post.Category;
            postEntity.Name = post.Name;
        }

        public async Task DeletePost(int id)
        {
            _repository.Remove(_repository.Get(id));
            _unitOfWork.Save();
        }
    }
}
