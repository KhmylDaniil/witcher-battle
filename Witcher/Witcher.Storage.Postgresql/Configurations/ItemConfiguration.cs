using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	public class ItemConfiguration : EntityBaseConfiguration<Item>
	{
		public override void ConfigureChild(EntityTypeBuilder<Item> builder)
		{
			builder.ToTable("Items", "Items").
				HasComment("Предметы");

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

			builder.Property(r => r.Quantity)
				.HasColumnName("Quantity")
				.HasComment("Количество")
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
				.WithMany(x => x.Items)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Characters)
				.WithMany(x => x.Items);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Item.Game));
			gameNavigation.SetField(Item.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
