using EcommerceAPI.Dto.Products;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Services.Products
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            // Use our specialized repository method to load child subcategories
            return await _categoryRepo.GetAllWithSubCategoriesAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            // Assuming your generic repository defines GetByIdAsync(id)
            return await _categoryRepo.GetByIdAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(CategoryDto dto)
        {
            var category = new Category
            {
                CategoryName = dto.CategoryName.Trim(),
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            // Using generic repository Add handler
            await _categoryRepo.AddAsync(category);
           // await _categoryRepo.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(int id, CategoryDto dto)
        {
            var existingCategory = await _categoryRepo.GetByIdAsync(id);
            if (existingCategory == null) return null;

            existingCategory.CategoryName = dto.CategoryName.Trim();
            existingCategory.Description = dto.Description;
            existingCategory.IsActive = dto.IsActive;

            // Using generic repository Update handler
            await _categoryRepo.Update(existingCategory);
            return existingCategory;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return false;

            // Using generic repository Delete handler
            await _categoryRepo.Delete(category);
            return true;
        }
    }
}
