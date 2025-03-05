using server.Data;
using server.Interfaces;

namespace server.Repositories;

public class Utilities : IUtilities
{
    private readonly ClothingStoreDbContext _context;
    private readonly ILogger<Utilities> _logger;

    public Utilities(ClothingStoreDbContext context,
                     ILogger<Utilities> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task PerformTransactionAsync(Func<Task> action)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await action();
            await transaction.CommitAsync();
            _logger.LogInformation("Transaction committed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Transaction failed. Rolling back.");
            await transaction.RollbackAsync();
            throw;
        }
    }
}