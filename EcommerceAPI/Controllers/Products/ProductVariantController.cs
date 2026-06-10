using EcommerceAPI.Dto.Products;
using EcommerceAPI.Interfaces.Services.Products;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers.Products
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductVariantController : ControllerBase
    {
        private readonly IProductVariantService _productVariantService;

        public ProductVariantController(IProductVariantService productVariantService)
        {
            _productVariantService = productVariantService;
        }


        // POST: api/productvariant
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductVariantDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _productVariantService.CreateVariantAsync(dto);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // PUT: api/productvariant/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] ProductVariantDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _productVariantService.UpdateVariantAsync(id, dto);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _productVariantService.GetAllVariantsAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error loading database arrays.", details = ex.Message });
            }
        }

        // DELETE: api/ProductVariant/3fa85f64-5717-4562-b3fc-2c963f66afa6
        [HttpDelete("{id:guid}")] // 👈 Route constraint forces structural Guid typing formats
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var deleted = await _productVariantService.DeleteVariantAsync(id);

                if (!deleted)
                {
                    return NotFound(new { message = "Variant entity record matching requested Guid could not be found." });
                }

                return Ok(new { message = "Product variant completely dropped from database schemas." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Database removal failed.", details = ex.Message });
            }
        }
    }
}
