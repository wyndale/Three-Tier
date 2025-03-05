using server.Models;

namespace server.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<Category?> GetByNameAsync(string name);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Guid id);
}
