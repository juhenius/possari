using Possari.Domain.Parents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Possari.Infrastructure.Parents.Persistence;

public class ParentConfigurations : IEntityTypeConfiguration<Parent>
{
  public void Configure(EntityTypeBuilder<Parent> builder)
  {
    builder.HasKey(g => g.Id);

    builder.Property(g => g.Id)
      .ValueGeneratedNever();

    builder.Property(g => g.Name);

    builder.Property(g => g.Version)
      .IsConcurrencyToken();
  }
}
