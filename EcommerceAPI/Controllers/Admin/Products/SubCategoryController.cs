using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Interfaces.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Controllers.Admin.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _subCategoryService.GetAllSubCategoriesAsync();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var subCategory = await _subCategoryService.GetSubCategoryByIdAsync(id);
            if (subCategory == null) return NotFound(new { message = "SubCategory record not found." });
            return Ok(subCategory);
        }

        // Fetch subcategories tied to a specific parent category ID
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategoryId(Guid categoryId)
        {
            var list = await _subCategoryService.GetSubCategoriesByCategoryIdAsync(categoryId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SubCategoryDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _subCategoryService.CreateSubCategoryAsync(dto);
                if (created == null)
                    return BadRequest(new { message = "Invalid Parent CategoryId specified." });

                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
            {
                return Conflict(new { message = "A subcategory with this designation already exists." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SubCategoryDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _subCategoryService.UpdateSubCategoryAsync(id, dto);
            if (updated == null)
                return NotFound(new { message = "Update rejected. Subcategory missing." });

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _subCategoryService.DeleteSubCategoryAsync(id);
            if (!success) return NotFound(new { message = "Subcategory not found." });
            return Ok(new { message = "Subcategory dropped successfully." });
        }
    }
}
