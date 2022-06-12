using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogCoreAPI.DTOs.Category;
using DBAccess.Data.POCO;
using DBAccess.Repositories.Category;
using DBAccess.Repositories.UnitOfWork;
using DBAccess.Specifications;
using DBAccess.Specifications.FilterSpecifications;
using DBAccess.Specifications.SortSpecification;

namespace BlogCoreAPI.Services.CategoryService
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

        public async Task<IEnumerable<GetCategoryDto>> GetCategories(FilterSpecification<Category> filterSpecification = null,
            PagingSpecification pagingSpecification = null,
            SortSpecification<Category> sortSpecification = null)
        {
            return (await _repository.GetAsync(filterSpecification, pagingSpecification, sortSpecification)).Select(x => _mapper.Map<GetCategoryDto>(x));
        }

        /// <inheritdoc />
        public async Task<int> CountCategoriesWhere(FilterSpecification<Category> filterSpecification = null)
        {
            return await _repository.CountWhereAsync(filterSpecification);
        }

        public async Task<GetCategoryDto> GetCategory(int id)
        {
            return _mapper.Map<GetCategoryDto>(await _repository.GetAsync(id));
        }

        public void CheckCategoryValidity(ICategoryDto category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            if (string.IsNullOrWhiteSpace(category.Name))
                throw new ArgumentException("Name cannot be null or empty.");
            if (category.Name.Length > 50)
                throw new ArgumentException("Name cannot exceed 50 characters.");
        }

        public async Task CheckCategoryValidity(AddCategoryDto category)
        {
            CheckCategoryValidity((ICategoryDto)category);
            if (await _repository.NameAlreadyExists(category.Name))
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task CheckCategoryValidity(UpdateCategoryDto category)
        {
            CheckCategoryValidity((ICategoryDto) category);
            if (await _repository.NameAlreadyExists(category.Name) &&
                (await _repository.GetAsync(category.Id)).Name != category.Name)
                throw new InvalidOperationException("Name already exists.");
        }

        public async Task<GetCategoryDto> AddCategory(AddCategoryDto category)
        {
            await CheckCategoryValidity(category);
            var result = await _repository.AddAsync(_mapper.Map<Category>(category));
            _unitOfWork.Save();
            return _mapper.Map<GetCategoryDto>(result);
        }

        private async Task<bool> CategoryAlreadyExistsWithSameProperties(UpdateCategoryDto category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
            var categoryDb = await _repository.GetAsync(category.Id);
            return categoryDb.Name == category.Name;
        }

        public async Task UpdateCategory(UpdateCategoryDto category)
        {
            if (await CategoryAlreadyExistsWithSameProperties(category))
                return;
            await CheckCategoryValidity(category);
            var categoryEntity = await _repository.GetAsync(category.Id);
            _mapper.Map(category, categoryEntity);
            _unitOfWork.Save();
        }

        public async Task DeleteCategory(int id)
        {
            await _repository.RemoveAsync(await _repository.GetAsync(id));
            _unitOfWork.Save();
        }
    }
}
