using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	public class WeaponConfiguration : HierarchyConfiguration<Weapon>
	{
		public override void ConfigureChild(EntityTypeBuilder<Weapon> builder)
		{
			builder.ToTable("Weapons", "Items").
				HasComment("Оружие");

			builder.Property(r => r.CurrentDurability)
				.HasColumnName("CurrentDurability")
				.HasComment("Текущая прочность")
				.IsRequired();
		}
	}
}
