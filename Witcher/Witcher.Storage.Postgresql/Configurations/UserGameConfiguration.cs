using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности пользователя игры
	/// </summary>
	public class UserGameConfiguration : EntityBaseConfiguration<UserGame>
	{
		/// <summary>
		/// Конфигурация для сущности пользователя игры
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<UserGame> builder)
		{
			builder.ToTable("UserGames", "BaseGame")
				.HasComment("Игры пользователя");

			builder.Property(x => x.UserId)
				.HasColumnName("UserId")
				.HasComment("Айди пользователя")
				.IsRequired();

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(x => x.InterfaceId)
				.HasColumnName("InterfaceId")
				.HasComment("Айди интерфейса")
				.IsRequired();

			builder.Property(x => x.GameRoleId)
				.HasColumnName("GameRoleId")
				.HasComment("Айди роли в игре")
				.IsRequired();

			builder.HasOne(x => x.User)
				.WithMany(x => x.UserGames)
				.HasForeignKey(x => x.UserId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Game)
				.WithMany(x => x.UserGames)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Interface)
				.WithMany(x => x.UserGames)
				.HasForeignKey(x => x.InterfaceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.GameRole)
				.WithMany(x => x.UserGames)
				.HasForeignKey(x => x.GameRoleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.Instances)
				.WithOne(x => x.UserGameActivated)
				.HasForeignKey(x => x.UserGameActivatedId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.UserGameCharacters)
				.WithOne(x => x.UserGame)
				.HasForeignKey(x => x.UserGameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(UserGame.Game));
			gameNavigation.SetField(UserGame.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var userNavigation = builder.Metadata.FindNavigation(nameof(UserGame.User));
			userNavigation.SetField(UserGame.UserField);
			userNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var interfaceNavigation = builder.Metadata.FindNavigation(nameof(UserGame.Interface));
			interfaceNavigation.SetField(UserGame.InterfaceField);
			interfaceNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameRoleNavigation = builder.Metadata.FindNavigation(nameof(UserGame.GameRole));
			gameRoleNavigation.SetField(UserGame.GameRoleField);
			gameRoleNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
