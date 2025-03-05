using AutoMapper;
using FluentValidation;
using server.DTOs;
using server.Exceptions;
using server.Interfaces;
using server.Models;

namespace server.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;
    private readonly IValidator<ProductsDTO> _validator;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUtilities _utilities;

    public ProductService(IProductRepository repository,
                          IMapper mapper,
                          ILogger<ProductService> logger,
                          IValidator<ProductsDTO> validator,
                          ICategoryRepository categoryRepository,
                          IUtilities utilities)
    {
        _productRepository = repository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
        _categoryRepository = categoryRepository;
        _utilities = utilities;
    }

    public async Task<IEnumerable<ProductsDTO>> GetAllProductsAsync()
    {
        _logger.LogInformation("Fetching all products from the repository.");

        var products = await _productRepository.GetAllAsync();
        if (!products.Any())
        {
            _logger.LogWarning("No products found in the repository.");
            throw new NoProductsAvailableException("No products available.");
        }

        return _mapper.Map<IEnumerable<ProductsDTO>>(products);
    }

    public async Task<ProductsDTO?> GetProductByIdAsync(Guid id)
    {
        _logger.LogInformation("Fetching product by ID: {Id}", id);

        var product = await _productRepository.GetByIdAsync(id);
        if (product == null)
        {
            _logger.LogWarning("Product with ID {Id} not found.", id);
            throw new ProductNotFoundException("Product not found.");
        }

        return _mapper.Map<ProductsDTO>(product);
    }

    public async Task AddProductAsync(ProductsDTO productDto)
    {
        _logger.LogInformation("Adding a new product: {Name}", productDto.Name);

        await _validator.ValidateAndThrowAsync(productDto);

        // Check if the category exists by name
        var category = await _categoryRepository.GetByNameAsync(productDto.CategoryName)
            ?? throw new CategoryNotFoundException("Category not found.");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            CategoryId = category.Id
        };

        await ExecuteTransactionAsync(async () => await _productRepository.AddAsync(product));
    }

    public async Task UpdateProductAsync(UpdateProductDTO productDto)
    {
        _logger.LogInformation("Updating product: {Id}", productDto.Id);

        var existingProduct = await _productRepository.GetByIdAsync(productDto.Id)
            ?? throw new ProductNotFoundException("Product not found.");

        var category = await _categoryRepository.GetByNameAsync(productDto.CategoryName)
            ?? throw new CategoryNotFoundException("Category not found.");

        existingProduct.Name = productDto.Name;
        existingProduct.Description = productDto.Description;
        existingProduct.Price = productDto.Price;
        existingProduct.CategoryId = category.Id;

        var updatedProduct = _mapper.Map(productDto, existingProduct);
        await ExecuteTransactionAsync(async () => await _productRepository.UpdateAsync(updatedProduct));
    }

    public async Task DeleteProductAsync(Guid id)
    {
        _logger.LogInformation("Deleting product with ID: {Id}", id);

        var existingProduct = await _productRepository.GetByIdAsync(id)
            ?? throw new ProductNotFoundException("Product not found.");

        await ExecuteTransactionAsync(async () => await _productRepository.DeleteAsync(id));
    }

    private async Task ExecuteTransactionAsync(Func<Task> action)
    {
        await _utilities.PerformTransactionAsync(action);
    }
}
