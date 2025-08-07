using VietDonate.Application.Common.Interfaces;

namespace VietDonate.Application.Common.Handlers
{
    public abstract class BaseCommandHandler
    {
        protected readonly IUnitOfWork _unitOfWork;

        protected BaseCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected async Task<T> ExecuteInTransactionAsync<T>(Func<Task<T>> operation)
        {
            await _unitOfWork.BeginTransactionAsync();
            
            try
            {
                var result = await operation();
                await _unitOfWork.CommitAsync();
                return result;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }

        protected async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            await _unitOfWork.BeginTransactionAsync();
            
            try
            {
                await operation();
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            finally
            {
                await _unitOfWork.DisposeAsync();
            }
        }
    }
}
