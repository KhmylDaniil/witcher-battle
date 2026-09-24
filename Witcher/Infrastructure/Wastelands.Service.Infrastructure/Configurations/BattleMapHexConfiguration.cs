using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class BattleMapHexConfiguration : EntityConfiguration<BattleMapHex>
	{
		public override void Configure(EntityTypeBuilder<BattleMapHex> builder)
		{
			base.Configure(builder);

			builder.ToTable("BattleMapHex");

			builder.Property(x => x.BattleMapId)
				.HasColumnName("BattleMapId")
				.IsRequired();

			builder.Property(x => x.Column)
				.HasColumnName("Column")
				.IsRequired();

			builder.Property(x => x.Row)
				.HasColumnName("Row")
				.IsRequired();

			builder.Property(x => x.TerrainType)
				.HasColumnName("TerrainType")
				.IsRequired();

			builder.Property(x => x.TerrainStyle)
				.HasColumnName("TerrainStyle")
				.IsRequired();

			builder.Ignore(x => x.IsPassable);
			builder.Ignore(x => x.MovementCost);

			// Одна клетка сетки — один гекс.
			builder.HasIndex(x => new { x.BattleMapId, x.Column, x.Row })
				.IsUnique();
		}
	}
}
