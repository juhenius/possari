using Possari.Domain.Children;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Possari.Infrastructure.Children.Persistence;

public class ChildConfigurations : IEntityTypeConfiguration<Child>
{
  public void Configure(EntityTypeBuilder<Child> builder)
  {
    builder.HasKey(g => g.Id);

    builder.Property(g => g.Id)
      .ValueGeneratedNever();

    builder.Property(g => g.Name);

    builder.Property(g => g.Version)
      .IsConcurrencyToken();

    builder.OwnsMany(c => c.PendingRewards, pb =>
    {
      pb.ToTable("PendingRewards");

      pb.HasKey(r => r.Id);

      pb.Property(r => r.Id)
        .ValueGeneratedNever();

      pb.WithOwner().HasForeignKey(r => r.ChildId);

      pb.Property(r => r.RewardName)
        .IsRequired();

      pb.Property(g => g.Version)
        .IsConcurrencyToken();
    });
  }
}
