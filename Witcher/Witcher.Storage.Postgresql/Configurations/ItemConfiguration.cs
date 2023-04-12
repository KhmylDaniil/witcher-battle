using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.Entities;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Storage.Postgresql.Configurations
{
	public class ItemConfiguration : EntityBaseConfiguration<Item>
	{
		public override void ConfigureChild(EntityTypeBuilder<Item> builder)
		{
			builder.ToTable("Items", "Items").
				HasComment("Предметы");

			builder.Property(r => r.BagId)
				.HasColumnName("BagId")
				.HasComment("Айди сумки")
				.IsRequired();

			builder.Property(r => r.ItemTemplateId)
				.HasColumnName("ItemTemplateId")
				.HasComment("Айди шаблона предмета")
				.IsRequired();

			builder.Property(r => r.Quantity)
				.HasColumnName("Quantity")
				.HasComment("Количество");

			builder.Property(r => r.ItemType)
				.HasColumnName("ItemType")
				.HasComment("Тип предмета")
				.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<ItemType>(v))
				.IsRequired();

			builder.HasOne(x => x.Bag)
				.WithMany(x => x.Items)
				.HasForeignKey(x => x.BagId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ItemTemplate)
				.WithMany(x => x.Exemplars)
				.HasForeignKey(x => x.ItemTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var bagNavigation = builder.Metadata.FindNavigation(nameof(Item.Bag));
			bagNavigation.SetField(Item.BagField);
			bagNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var itemTemplateNavigation = builder.Metadata.FindNavigation(nameof(Item.ItemTemplate));
			itemTemplateNavigation.SetField(Item.ItemTemplateField);
			itemTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
