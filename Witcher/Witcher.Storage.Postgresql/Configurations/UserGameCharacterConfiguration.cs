using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для персонажей пользователя игры
	/// </summary>
	public class UserGameCharacterConfiguration : EntityBaseConfiguration<UserGameCharacter>
	{
		/// <summary>
		/// Конфигурация для персонажей пользователя игры
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<UserGameCharacter> builder)
		{
			builder.ToTable("UserGameCharacters", "Characters")
				.HasComment("Персонажи пользователя игры");

			builder.Property(x => x.UserGameId)
				.HasColumnName("UserGameId")
				.HasComment("Айди пользователя игры")
				.IsRequired();

			builder.Property(x => x.CharacterId)
				.HasColumnName("CharacterId")
				.HasComment("Айди персонажа")
				.IsRequired();

			builder.HasOne(x => x.Character)
				.WithMany(x => x.UserGameCharacters)
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.UserGame)
				.WithMany(x => x.UserGameCharacters)
				.HasForeignKey(x => x.UserGameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ActivateCharacter)
				.WithOne(x => x.UserGameActivated)
				.HasForeignKey<Character>(x => x.UserGameActivatedId)
				.HasPrincipalKey<UserGameCharacter>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			var userGameNavigation = builder.Metadata.FindNavigation(nameof(UserGameCharacter.UserGame));
			userGameNavigation.SetField(UserGameCharacter.UserGameField);
			userGameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var characterNavigation = builder.Metadata.FindNavigation(nameof(UserGameCharacter.Character));
			characterNavigation.SetField(UserGameCharacter.CharacterField);
			characterNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
