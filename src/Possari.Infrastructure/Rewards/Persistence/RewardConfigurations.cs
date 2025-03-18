using Possari.Domain.Rewards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Possari.Infrastructure.Rewards.Persistence;

public class RewardConfigurations : IEntityTypeConfiguration<Reward>
{
  public void Configure(EntityTypeBuilder<Reward> builder)
  {
    builder.HasKey(g => g.Id);

    builder.Property(g => g.Id)
      .ValueGeneratedNever();

    builder.Property(g => g.Name);

    builder.Property(g => g.Version)
      .IsConcurrencyToken();
  }
}
