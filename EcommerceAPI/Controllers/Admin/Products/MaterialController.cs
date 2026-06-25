using EcommerceAPI.Dto.Admin.Products;
using EcommerceAPI.Interfaces.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Controllers.Admin.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        // Inject the Material Service Interface
        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _materialService.GetAllMaterialAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var material = await _materialService.GetMaterialByIdAsync(id);
            if (material == null) return NotFound(new { message = "Material not found" });
            return Ok(material);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MaterialDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var created = await _materialService.CreateMaterialAsync(dto);
                // Dynamically reads the generated Id from the returned DTO
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
            {
                return Conflict(new { message = "A material with this name already exists." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MaterialDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var updated = await _materialService.UpdateMaterialAsync(id, dto);
                if (updated == null) return NotFound(new { message = "Material not found" });
                return Ok(updated);
            }
            catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("UNIQUE") == true)
            {
                return Conflict(new { message = "Another material with this name already exists." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _materialService.DeleteMaterialAsync(id);
            if (!success) return NotFound(new { message = "Material not found" });
            return Ok(new { message = "Material deleted successfully." });
        }
    }
}