using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Interfaces.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Controllers.Admin.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        // Inject the Service Interface
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _categoryService.GetAllCategoriesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound(new { message = "Category not found" });
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var created = await _categoryService.CreateCategoryAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
            {
                return Conflict(new { message = "A category with this name already exists." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var updated = await _categoryService.UpdateCategoryAsync(id, dto);
                if (updated == null) return NotFound(new { message = "Category not found" });
                return Ok(updated);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
            {
                return Conflict(new { message = "Another category with this name already exists." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _categoryService.DeleteCategoryAsync(id);
            if (!success) return NotFound(new { message = "Category not found" });
            return Ok(new { message = "Category deleted successfully." });
        }
    }
}
