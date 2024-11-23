using EasyAccounts.DbContexts;
using EasyAccounts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Features;
using Microsoft.EntityFrameworkCore;

namespace EasyAccounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCategoryController : ControllerBase
    {
        private readonly EasyAccDbContext _context;
        private readonly ILogger _logger;

        public ItemCategoryController(EasyAccDbContext context, ILogger<ItemCategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("ItemCategory")]
        public async Task<IActionResult> GetItemCategory()
        {
            try
            {
                var ItemCategory = await _context.ItemCategories.Select(itc => new { itc.Id, itc.Name }).ToListAsync();
                return Ok(ItemCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ItemCategory Not Found");
                return StatusCode(500, $"Internal Server Error occured: {ex.Message}");
            }
        }

        [HttpPost("AddItemCategory")]

        public async Task<IActionResult> AdditemCategory([FromBody]ItemCategory newItemCategory)
        {
            if (newItemCategory == null || string.IsNullOrEmpty(newItemCategory.Name))
            {
                return BadRequest("Item category details required");
            }
            try
            {
                _context.ItemCategories.Add(newItemCategory);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetItemCategory), new { id = newItemCategory.Id }, newItemCategory);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex,"An error occured while adding the item category");
                return StatusCode(500, $"Internal server error : {ex.Message}");
            }

        }
    }
}
