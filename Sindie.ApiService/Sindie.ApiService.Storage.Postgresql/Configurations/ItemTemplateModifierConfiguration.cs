using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для Модификатора шаблона предмета
	/// </summary>
	public class ItemTemplateModifierConfiguration : EntityBaseConfiguration<ItemTemplateModifier>
	{
		/// <summary>
		/// Конфигурация для Модификатора шаблона предмета
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<ItemTemplateModifier> builder)
		{
			builder.ToTable("ItemTemplateModifiers", "GameRules").
				HasComment("Модификаторы шаблонов предметов");

			builder.Property(r => r.ModifierId)
				.HasColumnName("ModifierId")
				.HasComment("Айди модификатора")
				.IsRequired();

			builder.Property(r => r.ItemTemplateId)
				.HasColumnName("ItemTemplateId")
				.HasComment("Айди шаблона предмета")
				.IsRequired();

			builder.HasOne(x => x.Modifier)
				.WithMany(x => x.ItemTemplateModifiers)
				.HasForeignKey(x => x.ModifierId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ItemTemplate)
				.WithMany(x => x.ItemTemplateModifiers)
				.HasForeignKey(x => x.ItemTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigationModifier = builder.Metadata.FindNavigation(nameof(ItemTemplateModifier.Modifier));
			gameNavigationModifier.SetField(ItemTemplateModifier.ModifierField);
			gameNavigationModifier.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameNavigationItemTemplate = builder.Metadata.FindNavigation(nameof(ItemTemplateModifier.ItemTemplate));
			gameNavigationItemTemplate.SetField(ItemTemplateModifier.ItemTemplateField);
			gameNavigationItemTemplate.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
