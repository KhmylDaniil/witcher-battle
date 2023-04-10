using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	public class ItemTemplateConfiguration : EntityBaseConfiguration<ItemTemplate>
	{
		public override void ConfigureChild(EntityTypeBuilder<ItemTemplate> builder)
		{
			builder.ToTable("ItemTemplates", "Items").
				HasComment("Шаблоны предметов");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание");

			builder.Property(r => r.IsStackable)
				.HasColumnName("IsStackable")
				.IsRequired();

			builder.Property(r => r.Weight)
				.HasColumnName("Weight")
				.HasComment("Вес")
				.IsRequired();

			builder.Property(r => r.Price)
				.HasColumnName("Price")
				.HasComment("Цена")
				.IsRequired();

			builder.HasOne(x => x.Game)
				.WithMany(x => x.ItemTemplates)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Exemplars)
				.WithOne(x => x.ItemTemplate)
				.HasForeignKey(x => x.ItemTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(ItemTemplate.Game));
			gameNavigation.SetField(ItemTemplate.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
