using Possari.Domain.Primitives;

namespace Possari.Domain.Parents;

public sealed class Parent : AggregateRoot
{
  public static Result<Parent> Create(string name)
  {
    var nameError = ValidateName(name);
    if (nameError != Error.None)
    {
      return nameError;
    }

    var result = new Parent(name);

    result.RaiseDomainEvent(new ParentCreatedDomainEvent(result.Id));

    return result;
  }

  public string Name { get; private set; } = null!;

  private Parent(string name, Guid? id = null) : base(id)
  {
    Name = name;
  }

  private Parent() { }

  public Result Rename(string name)
  {
    var error = ValidateName(name);
    if (error != Error.None)
    {
      return error;
    }

    if (Name == name)
    {
      return Result.Success();
    }

    var previousName = Name;
    Name = name;

    RaiseDomainEvent(new ParentRenamedDomainEvent(Id, Name, previousName));

    return Result.Success();
  }

  public Result Delete()
  {
    RaiseDomainEvent(new ParentDeletedDomainEvent(Id));
    return Result.Success();
  }

  private static Error ValidateName(string name)
  {
    if (string.IsNullOrEmpty(name))
    {
      return ParentErrors.NameNullOrEmpty;
    }

    return Error.None;
  }
}
