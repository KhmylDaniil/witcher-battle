using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для персонажа
	/// </summary>
	public class CharacterConfiguration : HierarchyConfiguration<Character>
	{
		/// <summary>
		/// Конфигурация для персонажа
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Character> builder)
		{
			builder.ToTable("Characters", "Characters")
				.HasComment("Персонажи");

			builder.Property(x => x.UserGameActivatedId)
				.HasColumnName("UserGameActivatedId")
				.HasComment("Айди активировашего персонажа пользователя");


			builder.Property(x => x.ActivationTime)
				.HasColumnName("ActivationTime")
				.HasComment("Время активации персонажа");

			builder.HasOne(x => x.UserGameActivated)
				.WithOne(x => x.ActivateCharacter)
				.HasForeignKey<Character>(x => x.UserGameActivatedId)
				.HasPrincipalKey<UserGameCharacter>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.UserGameCharacters)
				.WithOne(x => x.Character)
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var userGameActivatedNavigation = builder.Metadata.FindNavigation(nameof(Character.UserGameActivated));
			userGameActivatedNavigation.SetField(Character.UserGameActivatedField);
			userGameActivatedNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
