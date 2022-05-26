using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для Слота шаблона пресонажа
	/// </summary>
	public class CharacterTemplateSlotConfiguration : EntityBaseConfiguration<CharacterTemplateSlot>
	{
		/// <summary>
		/// Конфигурация для Слота шаблона пресонажа
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<CharacterTemplateSlot> builder)
		{
			builder.ToTable("CharacterTemplateSlots", "GameRules").
				HasComment("Слоты шаблона персонажа");

			builder.Property(r => r.CharacterTemplateId)
				.HasColumnName("CharacterTemplateId")
				.HasComment("Айди шаблона персонажа")
				.IsRequired();

			builder.Property(r => r.SlotId)
				.HasColumnName("SlotId")
				.HasComment("Айди слота")
				.IsRequired();

			builder.HasOne(x => x.CharacterTemplate)
				.WithMany(x => x.CharacterTemplateSlots)
				.HasForeignKey(x => x.CharacterTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Slot)
				.WithMany(x => x.CharacterTemplateSlots)
				.HasForeignKey(x => x.SlotId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigationCharacterTemplate = builder.Metadata.FindNavigation(nameof(CharacterTemplateSlot.CharacterTemplate));
			gameNavigationCharacterTemplate.SetField(CharacterTemplateSlot.CharacterTemplateField);
			gameNavigationCharacterTemplate.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameNavigationSlot = builder.Metadata.FindNavigation(nameof(CharacterTemplateSlot.Slot));
			gameNavigationSlot.SetField(CharacterTemplateSlot.SlotField);
			gameNavigationSlot.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
