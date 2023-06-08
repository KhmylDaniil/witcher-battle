using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	public class ArmorTemplateConfiguration : HierarchyConfiguration<ArmorTemplate>
	{
		public override void ConfigureChild(EntityTypeBuilder<ArmorTemplate> builder)
		{
			builder.ToTable("ArmorTemplates", "Items").
				HasComment("Шаблоны брони");

			builder.Property(r => r.Armor)
				.HasColumnName("Armor")
				.HasComment("Броня")
				.IsRequired();

			builder.Property(r => r.EncumbranceValue)
				.HasColumnName("EncumbranceValue")
				.HasComment("Скованность движений")
				.IsRequired();

			builder.OwnsMany(x => x.DamageTypeModifiers)
				.HasKey(r => r.Id);

			builder.HasOne(x => x.BodyTemplate)
				.WithMany(x => x.ArmorsTemplates)
				.HasForeignKey(x => x.BodyTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.BodyTemplateParts)
				.WithMany(x => x.ArmorTemplates);

			var bodyTemplateNavigation = builder.Metadata.FindNavigation(nameof(ArmorTemplate.BodyTemplate));
			bodyTemplateNavigation.SetField(ArmorTemplate.BodyTemplateField);
			bodyTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

		}
	}
}
