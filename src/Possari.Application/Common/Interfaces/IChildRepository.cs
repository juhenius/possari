using Possari.Domain.Children;

namespace Possari.Application.Common.Interfaces;

public interface IChildRepository
{
  Task AddChildAsync(Child child);
  Task<Child?> GetByIdAsync(Guid childId);
  Task<List<Child>> ListAsync();
  Task RemoveChildAsync(Child child);
  Task UpdateChildAsync(Child child);
}
