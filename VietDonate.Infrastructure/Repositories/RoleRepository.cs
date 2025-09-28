using Microsoft.EntityFrameworkCore;

using VietDonate.Application.Common.Interfaces.IRepository;
using VietDonate.Infrastructure.Common.Persistance;

namespace VietDonate.Infrastructure.Repositories;

public class RoleRepository(
  AppDbContext context
) : IRoleRepository
{
  readonly AppDbContext _context = context;

  public Task<Guid> GetRoleIdByNameAsync(
    string roleName,
    CancellationToken cancellationToken
  )
  {
    return _context.Roles
      .Where(r => r.Name == roleName)
      .Select(r => r.Id)
      .FirstOrDefaultAsync(cancellationToken);
  }
}