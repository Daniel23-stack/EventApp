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
public class AlertsController : ControllerBase
{
    private readonly IAlertService _alertService;
    private readonly IHubContext<InventoryHub> _hubContext;

    public AlertsController(IAlertService alertService, IHubContext<InventoryHub> hubContext)
    {
        _alertService = alertService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlertDto>>> GetAllAlerts()
    {
        var alerts = await _alertService.GetActiveAlertsAsync();
        return Ok(alerts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AlertDto>> GetAlertById(int id)
    {
        var alert = await _alertService.GetAlertByIdAsync(id);
        if (alert == null)
        {
            return NotFound();
        }
        return Ok(alert);
    }

    [HttpGet("product/{productId}")]
    public async Task<ActionResult<IEnumerable<AlertDto>>> GetAlertsByProductId(int productId)
    {
        var alerts = await _alertService.GetAlertsByProductIdAsync(productId);
        return Ok(alerts);
    }

    [HttpPost]
    [Authorize(Policy = "ManagerOrAdmin")]
    public async Task<ActionResult<AlertDto>> CreateAlert([FromBody] CreateAlertRequest request)
    {
        try
        {
            var alert = await _alertService.CreateAlertAsync(request);
            
            // Broadcast real-time alert
            await _hubContext.Clients.All.SendAsync("NewAlert", alert);
            
            return CreatedAtAction(nameof(GetAlertById), new { id = alert.Id }, alert);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/resolve")]
    [Authorize(Policy = "ManagerOrAdmin")]
    public async Task<IActionResult> ResolveAlert(int id)
    {
        try
        {
            var userId = GetUserId();
            var result = await _alertService.ResolveAlertAsync(id, userId ?? 0);
            if (!result)
            {
                return NotFound();
            }
            
            // Broadcast resolution
            await _hubContext.Clients.All.SendAsync("AlertResolved", id);
            
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("check-reorder-levels")]
    [Authorize(Policy = "ManagerOrAdmin")]
    public async Task<IActionResult> CheckReorderLevels()
    {
        try
        {
            await _alertService.CheckReorderLevelsAsync();
            return Ok(new { message = "Reorder levels checked successfully" });
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

