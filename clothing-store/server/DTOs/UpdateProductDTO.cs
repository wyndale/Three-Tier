using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class UpdateProductDTO
{
    public Guid Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(250)]
    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string CategoryName { get; set; } = string.Empty;
}
