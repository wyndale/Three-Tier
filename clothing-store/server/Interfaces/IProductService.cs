using server.DTOs;
using server.Models;

namespace server.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductsDTO>> GetAllProductsAsync();
    Task<ProductsDTO?> GetProductByIdAsync(Guid id);
    Task AddProductAsync(ProductsDTO product);
    Task UpdateProductAsync(UpdateProductDTO product);
    Task DeleteProductAsync(Guid id);
}