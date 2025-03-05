namespace server.Interfaces;

public interface IUtilities
{
    Task PerformTransactionAsync(Func<Task> action);
}
