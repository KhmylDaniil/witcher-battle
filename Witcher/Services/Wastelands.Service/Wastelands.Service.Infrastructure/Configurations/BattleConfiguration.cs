using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class BattleConfiguration : EntityConfiguration<Battle>
	{
		public override void Configure(EntityTypeBuilder<Battle> builder)
		{
			base.Configure(builder);

			builder.ToTable("Battle");

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder.Property(x => x.Status)
				.HasColumnName("Status")
				.IsRequired();

			builder.HasMany(x => x.Creatures)
				.WithOne()
				.HasForeignKey(x => x.BattleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Characters)
				.WithOne()
				.HasForeignKey(x => x.BattleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
