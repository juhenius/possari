using Possari.Domain.Parents;

namespace Possari.Application.Common.Interfaces;

public interface IParentRepository
{
  Task AddParentAsync(Parent parent);
  Task<Parent?> GetByIdAsync(Guid parentId);
  Task<List<Parent>> ListAsync();
  Task RemoveParentAsync(Parent parent);
  Task UpdateParentAsync(Parent parent);
}
