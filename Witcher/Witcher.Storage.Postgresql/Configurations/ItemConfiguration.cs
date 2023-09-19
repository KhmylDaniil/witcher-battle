using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	public class ItemConfiguration : EntityBaseConfiguration<Item>
	{
		public override void ConfigureChild(EntityTypeBuilder<Item> builder)
		{
			builder.ToTable("Items", "Items").
				HasComment("Предметы");

			builder.Property(r => r.CharacterId)
				.HasColumnName("CharacterId")
				.HasComment("Айди персонажа")
				.IsRequired();

			builder.Property(r => r.ItemTemplateId)
				.HasColumnName("ItemTemplateId")
				.HasComment("Айди шаблона предмета")
				.IsRequired();

			builder.Property(r => r.Quantity)
				.HasColumnName("Quantity")
				.HasComment("Количество");

			builder.Property(r => r.ItemType).HasColumnName("ItemType").HasComment("Тип предмета")
				.IsRequired();

			builder.Property(r => r.IsEquipped)
				.HasColumnName("IsEquipped")
				.HasComment("Экипировано");

			builder.HasOne(x => x.Character)
				.WithMany(x => x.Items)
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ItemTemplate)
				.WithMany(x => x.Exemplars)
				.HasForeignKey(x => x.ItemTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var characterNavigation = builder.Metadata.FindNavigation(nameof(Item.Character));
			characterNavigation.SetField(Item.CharacterField);
			characterNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var itemTemplateNavigation = builder.Metadata.FindNavigation(nameof(Item.ItemTemplate));
			itemTemplateNavigation.SetField(Item.ItemTemplateField);
			itemTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
