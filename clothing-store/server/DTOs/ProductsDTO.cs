using System.ComponentModel.DataAnnotations;

namespace server.DTOs;

public class ProductsDTO
{
    public Guid Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(250)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    public string CategoryName { get; set; } = string.Empty; // Extra field for better frontend handling
}
