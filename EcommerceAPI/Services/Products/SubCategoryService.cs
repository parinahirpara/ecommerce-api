using AutoMapper;
using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Interfaces.Repositories.Products;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Models.Products;
using EcommerceAPI.Repositories.Products;

namespace EcommerceAPI.Services.Products
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository; 
        private readonly IMapper _mapper;

        public SubCategoryService(ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubCategoryDto>> GetAllSubCategoriesAsync()
        {
            var subCategories = await _subCategoryRepository.GetAllWithCategoryAsync();

            return _mapper.Map<IEnumerable<SubCategoryDto>>(subCategories);
        }
        

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoriesByCategoryIdAsync(Guid categoryId)
        {
            var subCategories = await _subCategoryRepository.GetSubCategoriesByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<SubCategoryDto>>(subCategories);
        }

        public async Task<SubCategoryDto?> GetSubCategoryByIdAsync(Guid id)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(id);
            if (subCategory == null) return null;

            return _mapper.Map<SubCategoryDto>(subCategory);
        }

        public async Task<SubCategoryDto?> CreateSubCategoryAsync(SubCategoryDto dto)
        {
            // 1. Verify the parent category physically exists in the database
            var parentExists = await _categoryRepository.GetByIdAsync(dto.CategoryId);
            if (parentExists == null) return null; // Reject mapping if orphan key

            var subCategory = _mapper.Map<SubCategory>(dto);
            subCategory.SubCategoryName = dto.SubCategoryName.Trim();
            subCategory.CreatedDate = DateTime.UtcNow;

            await _subCategoryRepository.AddAsync(subCategory); // Internally saves changes safely
            await _subCategoryRepository.SaveChangesAsync();

            return _mapper.Map<SubCategoryDto>(subCategory);
        }

        public async Task<SubCategoryDto?> UpdateSubCategoryAsync(Guid id, SubCategoryDto dto)
        {
            SubCategory existingSub = await _subCategoryRepository.GetByIdAsync(id);
            if (existingSub == null) return null;

            _mapper.Map(dto, existingSub);
            existingSub.SubCategoryName = dto.SubCategoryName.Trim();

            _subCategoryRepository.Update(existingSub);
            await _subCategoryRepository.SaveChangesAsync();

            return _mapper.Map<SubCategoryDto>(existingSub);
        }

        public async Task<bool> DeleteSubCategoryAsync(Guid id)
        {
            var subCategory = await _subCategoryRepository.GetByIdAsync(id);
            if (subCategory == null) return false;

            _subCategoryRepository.Delete(subCategory);
            await _subCategoryRepository.SaveChangesAsync();
            return true;
        }
    }
}
