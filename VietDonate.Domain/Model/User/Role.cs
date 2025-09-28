using VietDonate.Domain.Common;

namespace VietDonate.Domain.Model.User;

public class Role(
    Guid id,
    string name,
    string description
) : Entity(id)
{
    public string Name { get; } = name;
    public string Description { get; } = description;
}