using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Interfaces.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers.Admin.Products
{
    [ApiController]
    [Route("api/admin/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _productService.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var productDto = await _productService.GetByIdAsync(id);
            if (productDto == null) return NotFound(new { message = "Product not found" });
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdDto = await _productService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _productService.UpdateAsync(id, dto);
                return Ok(new { message = "Product updated successfully" });
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success =  await _productService.DeleteAsync(id);
            if (!success) return NotFound(new { message = "Product not found" });
            return Ok(new { message = "Product deleted successfully" });
        }

        
    }
}
