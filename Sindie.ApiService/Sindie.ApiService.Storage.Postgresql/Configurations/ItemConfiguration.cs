using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности предмет
	/// </summary>
	public class ItemConfiguration : HierarchyConfiguration<Item>
	{
		/// <summary>
		/// Конфигурация для сущности предмет
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Item> builder)
		{
			builder.ToTable("Items", "GameRules").
				HasComment("Предметы");

			builder.Property(r => r.SlotId)
				.HasColumnName("SlotId")
				.HasComment("Айди слота")
				.IsRequired();

			builder.Property(r => r.ItemTemplateId)
				.HasColumnName("ItemTemplateId")
				.HasComment("Айди шаблона предмета")
				.IsRequired();

			builder.Property(r => r.ScriptId)
				.HasColumnName("ScriptId")
				.HasComment("Айди скрипта");

			builder.Property(r => r.Weight)
				.HasColumnName("Weight")
				.HasComment("Вес")
				.IsRequired();

			builder.Property(r => r.MaxQuantity)
				.HasColumnName("MaxQuantity")
				.HasComment("Максимальное количество в стаке")
				.IsRequired();

			builder.HasOne(x => x.Slot)
				.WithMany(x => x.Items)
				.HasForeignKey(x => x.SlotId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ItemTemplate)
				.WithMany(x => x.Items)
				.HasForeignKey(x => x.ItemTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Script)
				.WithMany(x => x.Items)
				.HasForeignKey(x => x.ScriptId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.InteractionItems)
				.WithOne(x => x.Item)
				.HasForeignKey(x => x.ItemId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.BagItems)
				.WithOne(x => x.Item)
				.HasForeignKey(x => x.ItemId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Bodies)
				.WithOne(x => x.Item)
				.HasForeignKey(x => x.ItemId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			var navigationSlot = builder.Metadata.FindNavigation(nameof(Item.Slot));
			navigationSlot.SetField(Item.SlotField);
			navigationSlot.SetPropertyAccessMode(PropertyAccessMode.Field);

			var navigationItemTemplate = builder.Metadata.FindNavigation(nameof(Item.ItemTemplate));
			navigationItemTemplate.SetField(Item.ItemTemplateField);
			navigationItemTemplate.SetPropertyAccessMode(PropertyAccessMode.Field);

			var navigationScript = builder.Metadata.FindNavigation(nameof(Item.Script));
			navigationScript.SetField(Item.ScriptField);
			navigationScript.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
