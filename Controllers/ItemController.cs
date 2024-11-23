using EasyAccounts.DbContexts;
using EasyAccounts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyAccounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly EasyAccDbContext _context;
        private readonly ILogger _logger;

        public ItemController(EasyAccDbContext context, ILogger<ItemCategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("Items")]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                var items = await _context.Items.Select(i => new {i.ItemId,i.Name,i.Description,i.ItemCategory,i.IsActive
                }).ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching items");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("AddItem")]
        public async Task<IActionResult> AddItem([FromBody]Item newItem)
        {
            if (newItem == null)
            {
                return BadRequest("Item is null");
            }
            try
            {
                var categoriesExists = await _context.ItemCategories.AnyAsync(c => c.Id == newItem.ItemCategoryId);
                if (!categoriesExists)
                {
                    return BadRequest("Invalid ItemCategory.");
                }
                if (newItem.PurchaseOrderId.HasValue)
                {
                    var poExists = await _context.PurchaseOrders.AnyAsync(po => po.Id == newItem.PurchaseOrderId);
                    if (!poExists)
                    {
                        return BadRequest("Invalid PurchaseOrder ID");
                    }
                }
                if (newItem.GRNId.HasValue)
                {
                    var grnExists = await _context.GRNs.AnyAsync(grn => grn.Id == newItem.GRNId);
                    if (!grnExists)
                    {
                        return BadRequest("Invalid GRN ID");
                    }
                }

                newItem.CreatedDate = DateTime.Now;
                _context.Items.Add(newItem);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetItems), new { id = newItem.ItemId }, newItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while adding an Item");
                return StatusCode(500, $"Internal server error:{ex.Message}");
            }
        }

    }


}
