namespace VietDonate.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task DisposeAsync();
    }
}
