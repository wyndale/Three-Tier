using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Exceptions;
using server.Interfaces;
using server.Models;

public class ProductRepository : IProductRepository
{
    private readonly ClothingStoreDbContext _context;

    public ProductRepository(ClothingStoreDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking() // Optimizes read performance
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .AsNoTracking() // Avoids unnecessary tracking
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Product product)
    {
        if (product == null) throw new InvalidProductException(nameof(product));

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        if (product == null) throw new InvalidProductException(nameof(product));

        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) throw new ProductNotFoundException($"Product with ID {id} not found.");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}