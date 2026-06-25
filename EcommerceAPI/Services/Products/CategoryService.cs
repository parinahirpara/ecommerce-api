using AutoMapper;
using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Services.Products
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllWithSubCategoriesAsync();

            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto dto)
        {
            var category = _mapper.Map<Category>(dto);

            category.CategoryName = dto.CategoryName.Trim(); 
            category.CreatedDate = DateTime.UtcNow;

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto?> UpdateCategoryAsync(Guid id, CategoryDto dto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null) return null;

            _mapper.Map(dto, existingCategory);
            existingCategory.CategoryName = dto.CategoryName.Trim();

            _categoryRepository.Update(existingCategory);
            await _categoryRepository.SaveChangesAsync();

            return _mapper.Map<CategoryDto>(existingCategory);
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();

            return true;
        }
    }
}
