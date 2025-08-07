using Microsoft.EntityFrameworkCore.Storage;
using VietDonate.Application.Common.Interfaces;
using VietDonate.Infrastructure.Common.Persistance;

namespace VietDonate.Infrastructure.Common.Persistance
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public async Task DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
        }
    }
}
