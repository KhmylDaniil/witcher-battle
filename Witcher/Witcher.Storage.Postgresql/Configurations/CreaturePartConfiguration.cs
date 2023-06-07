using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="CreaturePart"/>
	/// </summary>
	public class CreaturePartConfiguration : HierarchyConfiguration<CreaturePart>
	{
		/// <summary>
		/// Конфигурация для <see cref="CreaturePart"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<CreaturePart> builder)
		{
			builder.ToTable("CreatureParts", "Battles").
				HasComment("Части существа");

			builder.Property(x => x.CreatureId)
				.HasColumnName("CreatureId")
				.HasComment("Айди существа")
				.IsRequired();

			builder.Property(x => x.StartingArmor)
				.HasColumnName("StartingArmor")
				.HasComment("Стартовая броня")
				.IsRequired();

			builder.Property(x => x.CurrentArmor)
				.HasColumnName("CurrentArmor")
				.HasComment("Текущая броня")
				.IsRequired();

			builder.HasOne(x => x.Creature)
				.WithMany(x => x.CreatureParts)
				.HasForeignKey(x => x.CreatureId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ArmorParts)
				.WithOne(x => x.CreaturePart)
				.HasForeignKey(x => x.CreaturePartId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var creatureNavigation = builder.Metadata.FindNavigation(nameof(CreaturePart.Creature));
			creatureNavigation.SetField(CreaturePart.CreatureField);
			creatureNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
