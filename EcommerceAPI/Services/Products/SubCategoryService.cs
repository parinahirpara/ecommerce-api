using EcommerceAPI.Dto.Products;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Models.Products;

namespace EcommerceAPI.Services.Products
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepo;
        private readonly ICategoryRepository _categoryRepo; // Verified parent helper instance

        public SubCategoryService(ISubCategoryRepository subCategoryRepo, ICategoryRepository categoryRepo)
        {
            _subCategoryRepo = subCategoryRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync()
        {
            return await _subCategoryRepo.GetAllWithCategoryAsync();
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId)
        {
            return await _subCategoryRepo.GetSubCategoriesByCategoryIdAsync(categoryId);
        }

        public async Task<SubCategory?> GetSubCategoryByIdAsync(int id)
        {
            return await _subCategoryRepo.GetByIdAsync(id);
        }

        public async Task<SubCategory?> CreateSubCategoryAsync(SubCategoryDto dto)
        {
            // 1. Verify the parent category physically exists in the database
            var parentExists = await _categoryRepo.GetByIdAsync(dto.CategoryId);
            if (parentExists == null) return null; // Reject mapping if orphan key

            var subCategory = new SubCategory
            {
                CategoryId = dto.CategoryId,
                SubCategoryName = dto.SubCategoryName.Trim(),
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedDate = DateTime.UtcNow
            };

            await _subCategoryRepo.AddAsync(subCategory); // Internally saves changes safely
            return subCategory;
        }

        public async Task<SubCategory?> UpdateSubCategoryAsync(int id, SubCategoryDto dto)
        {
            var existingSub = await _subCategoryRepo.GetByIdAsync(id);
            if (existingSub == null) return null;

            // Verify parent structural change safety
            var parentExists = await _categoryRepo.GetByIdAsync(dto.CategoryId);
            if (parentExists == null) return null;

            existingSub.CategoryId = dto.CategoryId;
            existingSub.SubCategoryName = dto.SubCategoryName.Trim();
            existingSub.Description = dto.Description;
            existingSub.IsActive = dto.IsActive;

            await _subCategoryRepo.Update(existingSub);
            return existingSub;
        }

        public async Task<bool> DeleteSubCategoryAsync(int id)
        {
            var subCategory = await _subCategoryRepo.GetByIdAsync(id);
            if (subCategory == null) return false;

            await _subCategoryRepo.Delete(subCategory);
            return true;
        }
    }
}
