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

			builder.Property(x => x.CurrentRound)
				.HasColumnName("CurrentRound")
				.IsRequired();

			builder.Property(x => x.CurrentInitiative)
				.HasColumnName("CurrentInitiative")
				.IsRequired(false);

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

			builder.HasOne(x => x.Attack)
				.WithOne()
				.HasForeignKey<BattleAttack>(x => x.BattleId)
				.HasPrincipalKey<Battle>(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.LogEntries)
				.WithOne()
				.HasForeignKey(x => x.BattleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Property(x => x.BattleMapId)
				.HasColumnName("BattleMapId")
				.IsRequired(false);

			// SetNull: удаление карты не удаляет бой, он просто остаётся без карты.
			builder.HasOne<BattleMap>()
				.WithMany()
				.HasForeignKey(x => x.BattleMapId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);
		}
	}
}
