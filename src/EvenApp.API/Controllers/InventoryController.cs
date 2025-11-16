using EvenApp.Application.DTOs;
using EvenApp.Application.Services;
using EvenApp.Infrastructure.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace EvenApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly IHubContext<InventoryHub> _hubContext;

    public InventoryController(IInventoryService inventoryService, IHubContext<InventoryHub> hubContext)
    {
        _inventoryService = inventoryService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InventoryDto>>> GetAllInventory()
    {
        var inventory = await _inventoryService.GetAllInventoryAsync();
        return Ok(inventory);
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<InventoryDto>> GetInventoryByProductId(int productId)
    {
        var inventory = await _inventoryService.GetInventoryByProductIdAsync(productId);
        if (inventory == null)
        {
            return NotFound();
        }
        return Ok(inventory);
    }

    [HttpGet("location/{location}")]
    public async Task<ActionResult<IEnumerable<InventoryDto>>> GetInventoryByLocation(string location)
    {
        var inventory = await _inventoryService.GetInventoryByLocationAsync(location);
        return Ok(inventory);
    }

    [HttpGet("low-stock")]
    public async Task<ActionResult<IEnumerable<InventoryDto>>> GetLowStockItems()
    {
        var inventory = await _inventoryService.GetLowStockItemsAsync();
        return Ok(inventory);
    }

    [HttpPut]
    [Authorize(Policy = "ManagerOrAdmin")]
    public async Task<ActionResult<InventoryDto>> UpdateInventory([FromBody] UpdateInventoryRequest request)
    {
        try
        {
            var userId = GetUserId();
            var inventory = await _inventoryService.UpdateInventoryAsync(request, userId);
            
            // Broadcast real-time update
            await _hubContext.Clients.All.SendAsync("InventoryUpdated", inventory);
            
            return Ok(inventory);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("adjust")]
    [Authorize(Policy = "ManagerOrAdmin")]
    public async Task<ActionResult<InventoryDto>> AdjustInventory([FromBody] InventoryAdjustmentRequest request)
    {
        try
        {
            var userId = GetUserId();
            var inventory = await _inventoryService.AdjustInventoryAsync(request, userId);
            
            // Broadcast real-time update
            await _hubContext.Clients.All.SendAsync("InventoryUpdated", inventory);
            
            return Ok(inventory);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private int? GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? User.FindFirst("sub")?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}

