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

			// SetNull, а не Cascade: если игру снесли, персонажи игроков не удаляются вместе с ней —
			// они остаются у владельца в архивном виде (GameId становится null).
			builder.HasMany(x => x.Characters)
				.WithOne()
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.UserGames)
				.WithOne()
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.GameJoinRequests)
				.WithOne()
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.BodyTemplates)
				.WithOne()
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureTemplates)
				.WithOne()
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			// Cascade — бои не персонажи, архивировать нечего: удаление игры сносит все её бои.
			builder.HasMany(x => x.Battles)
				.WithOne()
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.BattleMaps)
				.WithOne()
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
