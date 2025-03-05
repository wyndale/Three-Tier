using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Exceptions;
using server.Interfaces;
using server.Models;

namespace server.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ClothingStoreDbContext _context;

    public CategoryRepository(ClothingStoreDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        var existingCategory = await _context.Categories.FindAsync(category.Id)
            ?? throw new CategoryNotFoundException("Category not found.");

        _context.Entry(existingCategory).CurrentValues.SetValues(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var category = await _context.Categories.FindAsync(id)
            ?? throw new CategoryNotFoundException("Category not found.");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}

