using EcommerceAPI.Dto.Products;
using EcommerceAPI.Interfaces.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers.Products
{
   [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _productService.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            await _productService.AddAsync(dto);
            return Ok(new { message = "Product added successfully" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductDto dto)
        {
            await _productService.UpdateAsync(id, dto);
            return Ok(new { message = "Product updated successfully" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);
            return Ok(new { message = "Product deleted successfully" });
        }

        
    }
}
