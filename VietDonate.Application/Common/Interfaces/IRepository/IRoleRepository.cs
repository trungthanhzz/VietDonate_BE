namespace VietDonate.Application.Common.Interfaces.IRepository;

public interface IRoleRepository
{
  Task<Guid> GetRoleIdByNameAsync(string roleName, CancellationToken cancellationToken);
}