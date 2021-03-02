using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DbAccess.Data.POCO;
using DbAccess.Repositories.Category;
using DbAccess.Repositories.UnitOfWork;
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
            return (await _repository.GetAllAsync()).Select(x => _mapper.Map<GetCategoryDto>(x)).ToList();
        }

        public async Task<GetCategoryDto> GetCategory(int id)
        {
            try
            {
                return _mapper.Map<GetCategoryDto>(await _repository.GetAsync(id));
            }
            catch (InvalidOperationException)
            {
                throw new IndexOutOfRangeException("Category doesn't exist.");
            }
        }

        public async Task CheckCategoryValidity(AddCategoryDto category)
        {
            if (category == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (category.Name.Length > 50)
                throw new ArgumentException("Name cannot exceed 50 characters.");
            if (await _repository.NameAlreadyExists(category.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task CheckCategoryValidity(UpdateCategoryDto category)
        {
            if (category == null)
                throw new ArgumentNullException();
            if (_repository.GetAsync(category.Id) == null)
                throw new ArgumentException("Category doesn't exist.");
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (category.Name.Length > 50)
                throw new ArgumentException("Name cannot exceed 50 characters.");
            if (await _repository.NameAlreadyExists(category.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task<GetCategoryDto> AddCategory(AddCategoryDto category)
        {
            await CheckCategoryValidity(category);
            var result = await _repository.AddAsync(_mapper.Map<Category>(category));
            _unitOfWork.Save();
            return _mapper.Map<GetCategoryDto>(result);
        }

        public async Task UpdateCategory(UpdateCategoryDto category)
        {
            await CheckCategoryValidity(category);
            try
            {
                //TODO
                var categoryEntity = await _repository.GetAsync(category.Id);
                _mapper.Map(category, categoryEntity);
                _unitOfWork.Save();
            }
            catch (InvalidOperationException)
            {
                throw new IndexOutOfRangeException("Category doesn't exist.");
            }
        }

        public async Task DeleteCategory(int id)
        {
            try
            {
                await _repository.RemoveAsync(await _repository.GetAsync(id));
                _unitOfWork.Save();
            }
            catch (InvalidOperationException)
            {
                throw new IndexOutOfRangeException("Category doesn't exist.");
            }

        }
    }
}
