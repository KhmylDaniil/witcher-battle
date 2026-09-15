using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.DAL.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="CreatureTemplatePart"/>
	/// </summary>
	public class CreatureTemplatePartConfiguration : HierarchyConfiguration<CreatureTemplatePart>
	{
		/// <summary>
		/// Конфигурация для <see cref="CreatureTemplatePart"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<CreatureTemplatePart> builder)
		{
			builder.ToTable("CreatureTemplateParts", "GameRules").
				HasComment("Части шаблона существа");

			builder.Property(x => x.CreatureTemplateId)
				.HasColumnName("CreatureTemplateId")
				.HasComment("Айди шаблона существа")
				.IsRequired();

			builder.Property(x => x.Armor)
				.HasColumnName("Armor")
				.HasComment("Броня")
				.IsRequired();

			builder.HasOne(x => x.CreatureTemplate)
				.WithMany(x => x.CreatureTemplateParts)
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var creatureTemplateNavigation = builder.Metadata.FindNavigation(nameof(CreatureTemplatePart.CreatureTemplate));
			creatureTemplateNavigation.SetField(CreatureTemplatePart.CreatureTemplateField);
			creatureTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
