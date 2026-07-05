using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Dto.Admin.Page;
using EcommerceAPI.Interfaces.Services.Page;

namespace EcommerceAPI.Controllers.Admin
{
    [ApiController]
    [Route("api/[controller]")]
    public class PageHeaderController : ControllerBase
    {
        private readonly IPageHeaderService _pageHeaderService;

        public PageHeaderController(IPageHeaderService pageHeaderService)
        {
            _pageHeaderService = pageHeaderService;
        }

        // 1. GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAllConfigs()
        {
            try
            {
                var result = await _pageHeaderService.GetAllPageHeadersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // 2. GET BY KEY
        [HttpGet("{pageKey}")]
        public async Task<IActionResult> GetConfig(string pageKey)
        {
            var result = await _pageHeaderService.GetPageHeaderAsync(pageKey);
            if (result == null)
                return NotFound(new { message = "Configuration record layout missing." });

            return Ok(result);
        }

        // 3. CREATE (ADD NEW)
        [HttpPost]
        public async Task<IActionResult> CreateConfig([FromForm] PageHeaderCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _pageHeaderService.CreatePageHeaderAsync(dto);
                return CreatedAtAction(nameof(GetConfig), new { pageKey = result.PageKey }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConfig(Guid id, [FromForm] PageHeaderCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _pageHeaderService.UpdatePageHeaderAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}