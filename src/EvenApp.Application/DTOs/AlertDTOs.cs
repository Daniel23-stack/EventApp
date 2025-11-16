namespace EvenApp.Application.DTOs;

public class AlertDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductSKU { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsResolved { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}

public class CreateAlertRequest
{
    public string Type { get; set; } = string.Empty;
    public int? ProductId { get; set; }
    public string Message { get; set; } = string.Empty;
}

