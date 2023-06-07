using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	public class ArmorPartConfiguration : EntityBaseConfiguration<ArmorPart>
	{
		public override void ConfigureChild(EntityTypeBuilder<ArmorPart> builder)
		{
			builder.ToTable("ArmorParts", "Items").
				HasComment("Части брони");

			builder.Property(x => x.CurrentArmor)
				.HasColumnName("CurrentArmor")
				.HasComment("Текущая броня")
				.IsRequired();

			builder.HasOne(r => r.Armor)
				.WithMany(x => x.ArmorParts)
				.HasForeignKey(x => x.ArmorId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(r => r.CreaturePart)
				.WithMany(x => x.ArmorParts)
				.HasForeignKey(x => x.CreaturePartId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var armorNavigation = builder.Metadata.FindNavigation(nameof(ArmorPart.Armor));
			armorNavigation.SetField(ArmorPart.ArmorField);
			armorNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var creaturePartNavigation = builder.Metadata.FindNavigation(nameof(ArmorPart.CreaturePart));
			creaturePartNavigation.SetField(ArmorPart.CreaturePartField);
			creaturePartNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
