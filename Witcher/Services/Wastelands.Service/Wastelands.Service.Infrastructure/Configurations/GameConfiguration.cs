using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class GameConfiguration : EntityConfiguration<Game>
	{
		public override void Configure(EntityTypeBuilder<Game> builder)
		{
			base.Configure(builder);

			builder.ToTable("Game");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder.Property(x => x.CreatedByUserId)
				.HasColumnName("CreatedByUserId")
				.IsRequired();

			builder.HasMany(x => x.Characters)
				.WithOne()
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
