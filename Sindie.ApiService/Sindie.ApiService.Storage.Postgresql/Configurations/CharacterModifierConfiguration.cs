using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация модификаторов персонажа
	/// </summary>
	public class CharacterModifierConfiguration : EntityBaseConfiguration<CharacterModifier>
	{
		/// <summary>
		/// Конфигурация модификаторов персонажа
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<CharacterModifier> builder)
		{
			builder.ToTable("CharacterModifiers", "GameInstance")
				.HasComment("Модификаторы персонажа");

			builder.Property(x => x.CharacterId)
				.HasColumnName("CharacterId")
				.HasComment("Айди персонажа")
				.IsRequired();

			builder.Property(x => x.ModifierId)
				.HasColumnName("ModifierId")
				.HasComment("Айди модификатора")
				.IsRequired();

			builder.HasOne(x => x.Character)
				.WithMany(x => x.CharacterModifiers)
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Modifier)
				.WithMany(x => x.CharacterModifiers)
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var characterFileNavigation = builder.Metadata.FindNavigation(nameof(CharacterModifier.Character));
			characterFileNavigation.SetField(CharacterModifier.CharacterField);
			characterFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var modifierFileNavigation = builder.Metadata.FindNavigation(nameof(CharacterModifier.Modifier));
			modifierFileNavigation.SetField(CharacterModifier.ModifierField);
			modifierFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
