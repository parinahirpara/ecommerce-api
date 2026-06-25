using EcommerceAPI.Dto.Common;
using EcommerceAPI.Dto.Customer.Products;
using EcommerceAPI.Interfaces.Services.Products;
using EcommerceAPI.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers.Customer
{
    [ApiController]
    [Route("api/customer/[controller]")]
    public class ProductController :ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductResponseDto>>> GetProductsDetails([FromQuery] int skip = 0, [FromQuery] int take = 12, [FromQuery] Guid? categoryId = null,[FromQuery] Guid ? subCategoryId = null) {
            var result = await _productService.GetProductsAsync(skip, take, categoryId, subCategoryId);
            return Ok(result);
        }
    }
}
