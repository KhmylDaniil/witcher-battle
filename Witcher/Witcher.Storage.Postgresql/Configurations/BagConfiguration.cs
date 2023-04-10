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
	public class BagConfiguration : EntityBaseConfiguration<Bag>
	{
		public override void ConfigureChild(EntityTypeBuilder<Bag> builder)
		{
			builder.ToTable("Bags", "Characters").
				HasComment("Сумки");

			builder.Property(r => r.CharacterId)
				.HasColumnName("CharacterId")
				.HasComment("Айди персонажа")
				.IsRequired();

			builder.Property(r => r.MaxWeight)
				.HasColumnName("MaxWeight")
				.HasComment("Макимальный вес")
				.IsRequired();

			builder.Property(r => r.TotalWeight)
				.HasColumnName("TotalWeight")
				.HasComment("Общий вес")
				.IsRequired();

			builder.HasOne(x => x.Character)
				.WithOne(x => x.Bag)
				.HasForeignKey<Bag>(x => x.CharacterId)
				.HasPrincipalKey<Character>(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Items)
				.WithOne(x => x.Bag)
				.HasForeignKey(x => x.BagId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var characterNavigation = builder.Metadata.FindNavigation(nameof(Bag.Character));
			characterNavigation.SetField(Bag.CharacterField);
			characterNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
