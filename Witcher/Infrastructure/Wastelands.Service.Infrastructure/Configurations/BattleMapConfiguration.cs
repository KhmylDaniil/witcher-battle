using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class BattleMapConfiguration : EntityConfiguration<BattleMap>
	{
		public override void Configure(EntityTypeBuilder<BattleMap> builder)
		{
			base.Configure(builder);

			builder.ToTable("BattleMap");

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder.Property(x => x.Description)
				.HasColumnName("Description")
				.HasColumnType("varchar(500)")
				.IsRequired(false);

			builder.Property(x => x.Columns)
				.HasColumnName("Columns")
				.IsRequired();

			builder.Property(x => x.Rows)
				.HasColumnName("Rows")
				.IsRequired();

			builder.HasMany(x => x.Hexes)
				.WithOne()
				.HasForeignKey(x => x.BattleMapId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
