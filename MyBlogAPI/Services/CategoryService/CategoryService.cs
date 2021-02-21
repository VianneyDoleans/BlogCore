using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.UnitOfWork;
using MyBlogAPI.DTO;
using MyBlogAPI.DTO.Category;

namespace MyBlogAPI.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetCategoryDto>> GetAllCategories()
        {
            return _repository.GetAll().Select(x => _mapper.Map<GetCategoryDto>(x)).ToList();
        }

        public async Task<GetCategoryDto> GetCategory(int id)
        {
            return _mapper.Map<GetCategoryDto>(_repository.Get(id));
        }

        public async Task<GetCategoryDto> AddCategory(AddCategoryDto category)
        {
            var result = _repository.Add(_mapper.Map<Category>(category));
            _unitOfWork.Save();
            return _mapper.Map<GetCategoryDto>(result);
        }

        public async Task UpdateCategory(UpdateCategoryDto category)
        {
            var categoryEntity = _repository.Get(category.Id);
            categoryEntity.Name = category.Name;
            //TODO
            _unitOfWork.Save();
        }

        public async Task DeleteCategory(int id)
        {
            _repository.Remove(_repository.Get(id));
            _unitOfWork.Save();
        }
    }
}
