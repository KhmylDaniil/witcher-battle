using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для Модификатора шаблона персонажа
	/// </summary>
	public class CharacterTemplateModifierConfiguration : EntityBaseConfiguration<CharacterTemplateModifier>
	{
		/// <summary>
		/// Конфигурация для Модификатора шаблона персонажа
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<CharacterTemplateModifier> builder)
		{
			builder.ToTable("CharacterTemplateModifiers", "GameRules").
				HasComment("Модификаторы шаблонов персонажей");

			builder.Property(r => r.ModifierId)
				.HasColumnName("ModifierId")
				.HasComment("Айди модификатора")
				.IsRequired();

			builder.Property(r => r.CharacterTemplateId)
				.HasColumnName("CharacterTemplateId")
				.HasComment("Айди шаблона персонажа")
				.IsRequired();

			builder.HasOne(x => x.Modifier)
				.WithMany(x => x.CharacterTemplateModifiers)
				.HasForeignKey(x => x.ModifierId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.CharacterTemplate)
				.WithMany(x => x.CharacterTemplateModifiers)
				.HasForeignKey(x => x.CharacterTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigationModifier = builder.Metadata.FindNavigation(nameof(CharacterTemplateModifier.Modifier));
			gameNavigationModifier.SetField(CharacterTemplateModifier.ModifierField);
			gameNavigationModifier.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameNavigationCharacterTemplate = builder.Metadata.FindNavigation(nameof(CharacterTemplateModifier.CharacterTemplate));
			gameNavigationCharacterTemplate.SetField(CharacterTemplateModifier.CharacterTemplateField);
			gameNavigationCharacterTemplate.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
