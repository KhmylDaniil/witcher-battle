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
	public class WeaponConfiguration : HierarchyConfiguration<Weapon>
	{
		public override void ConfigureChild(EntityTypeBuilder<Weapon> builder)
		{
			builder.ToTable("Weapons", "Items").
				HasComment("Оружие");

			builder.Property(r => r.EquippedByCharacterId)
				.HasColumnName("EquippedByCharacterId")
				.HasComment("Экипировавший персонаж");

			builder.Property(r => r.CurrentDurability)
				.HasColumnName("CurrentDurability")
				.HasComment("Текущая прочность")
				.IsRequired();

			builder.HasOne(x => x.EquippedByCharacter)
				.WithMany(x => x.EquippedWeapons)
				.HasForeignKey(x => x.EquippedByCharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			var equippedByNavigation = builder.Metadata.FindNavigation(nameof(Weapon.EquippedByCharacter));
			equippedByNavigation.SetField(Weapon.EquippedByCharacterField);
			equippedByNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
