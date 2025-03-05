using AutoMapper;
using server.DTOs;
using server.Exceptions;
using server.Interfaces;
using server.Models;

namespace server.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryService> _logger;
    private readonly IUtilities _utilities;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository,
                           ILogger<CategoryService> logger,
                           IUtilities utilities,
                           IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
        _utilities = utilities;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        _logger.LogInformation("Fetching all categories from the repository.");

        var categories = await _categoryRepository.GetAllAsync();

        if (!categories.Any())
        {
            _logger.LogWarning("No categories found in the repository.");

            throw new CategoryNotFoundException("No categories found.");
        }

        return categories;
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid id)
    {
        _logger.LogInformation("Fetching category by ID: {Id}", id);

        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            _logger.LogWarning("Category with ID {Id} not found.", id);

            throw new CategoryNotFoundException("No categories found.");
        }

        return category;
    }

    public async Task AddCategoryAsync(CategoryDTO categoryDto)
    {
        _logger.LogInformation("Adding a new category: {Name}", categoryDto.Name);

        var existingCategory = await _categoryRepository.GetByNameAsync(categoryDto.Name);
        if (existingCategory != null)
        {
            _logger.LogWarning("Category '{Name}' already exists.", categoryDto.Name);

            throw new CategoryAlreadyExistException($"Category '{categoryDto.Name}' already exists.");
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = categoryDto.Name
        };

        await ExecuteTransactionAsync(async () => await _categoryRepository.AddAsync(category));

        _logger.LogInformation("Category '{Name}' added successfully.", category.Name);
    }

    public async Task UpdateCategoryAsync(CategoryDTO category)
    {
        _logger.LogInformation("Updating category with ID: {Id}", category.Id);

        var existingCategory = await _categoryRepository.GetByIdAsync(category.Id);
        if (existingCategory == null)
        {
            _logger.LogWarning("Category with ID {Id} not found.", category.Id);

            throw new CategoryNotFoundException($"Category with ID {category.Id} not found.");
        }

        existingCategory.Name = category.Name;

        await ExecuteTransactionAsync(async () => await _categoryRepository.UpdateAsync(existingCategory));

        _logger.LogInformation("Category '{Id}' updated successfully.", category.Id);
    }

    public async Task DeleteCategoryAsync(Guid id)
    {
        _logger.LogInformation("Deleting category with ID: {Id}", id);

        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            _logger.LogWarning("Category with ID {Id} not found.", id);

            throw new CategoryNotFoundException($"Category with ID {id} not found.");
        }

        await ExecuteTransactionAsync(async () => await _categoryRepository.DeleteAsync(id));

        _logger.LogInformation("Category '{Id}' deleted successfully.", id);
    }

    private async Task ExecuteTransactionAsync(Func<Task> action)
    {
        await _utilities.PerformTransactionAsync(action);
    }
}

