using Microsoft.AspNetCore.Mvc;
using server.DTOs;
using server.Interfaces;

namespace server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService,
                             ILogger<ProductController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        _logger.LogInformation("Fetching all products.");

        var products = await _productService.GetAllProductsAsync();

        return Ok(products);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        _logger.LogInformation("Fetching product by ID: {id}", id);

        var product = await _productService.GetProductByIdAsync(id);

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddNewProduct(ProductsDTO productsDTO)
    {
        _logger.LogInformation("Attempting to add new product.");

        await _productService.AddProductAsync(productsDTO);

        return Ok(new { Success = "Product added successfully." });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductDTO updateProductDTO)
    {
        _logger.LogInformation("Attempting to update the product by ID: {id}", id);

        updateProductDTO.Id = id;

        if (id != updateProductDTO.Id)
        {
            return BadRequest("ID in the URL does not match ID in the request body.");
        }

        await _productService.UpdateProductAsync(updateProductDTO);

        return Ok(new { Success = "Product updated successfully." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        _logger.LogInformation("Attempting to delete the product by ID: {id}", id);

        await _productService.DeleteProductAsync(id);

        return Ok(new { Success = "Product deleted successfully." });
    }
}