using server.DTOs;
using server.Models;

namespace server.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task<Category?> GetCategoryByIdAsync(Guid id);
    Task AddCategoryAsync(CategoryDTO category);
    Task UpdateCategoryAsync(CategoryDTO category);
    Task DeleteCategoryAsync(Guid id);
}
