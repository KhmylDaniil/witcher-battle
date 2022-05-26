using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности предметы в сумке
	/// </summary>
	public class BagItemConfiguration : EntityBaseConfiguration<BagItem>
	{
		/// <summary>
		/// Конфигурация для сущности предметы в сумке
		/// </summary>
		/// <param name="builder">строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<BagItem> builder)
		{
			builder.ToTable("BagItems", "GameInstance")
				.HasComment("Предметы в сумке");
			
			builder.Property(r => r.BagId)
				.HasColumnName("BagId")
				.HasComment("Айди сумки")
				.IsRequired();

			builder.Property(r => r.ItemId)
				.HasColumnName("ItemId")
				.HasComment("Айди предмета")
				.IsRequired();

			builder.Property(r => r.QuantityItem)
				.HasColumnName("QuantityItem")
				.HasComment("Количество предметов в стеке")
				.IsRequired();

			builder.Property(r => r.MaxQuantityItem)
				.HasColumnName("MaxQuantityItem")
				.HasComment("Максимальное количество в стаке")
				.IsRequired();

			builder.Property(r => r.Stack)
				.HasColumnName("Stack")
				.HasComment("Стек")
				.IsRequired();

			builder.Property(r => r.Weight)
				.HasColumnName("Weight")
				.HasComment("Вес")
				.IsRequired();

			builder.Property(r => r.Blocked)
				.HasColumnName("Blocked")
				.HasComment("Заблокировано")
				.IsRequired();

			builder.HasOne(x => x.Bag)
				.WithMany(x => x.BagItems)
				.HasForeignKey(x => x.BagId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
			
			builder.HasOne(x => x.Item)
				.WithMany(x => x.BagItems)
				.HasForeignKey(x => x.ItemId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var bagNavigation = builder.Metadata.FindNavigation(nameof(BagItem.Bag));
			bagNavigation.SetField(BagItem.BagField);
			bagNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var itemNavigation = builder.Metadata.FindNavigation(nameof(BagItem.Item));
			itemNavigation.SetField(BagItem.ItemField);
			itemNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
