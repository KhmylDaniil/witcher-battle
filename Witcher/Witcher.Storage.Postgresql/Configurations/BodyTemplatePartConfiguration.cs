using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="BodyTemplatePart"/>
	/// </summary>
	public class BodyTemplatePartConfiguration : HierarchyConfiguration<BodyTemplatePart>
	{
		/// <summary>
		/// Конфигурация для <see cref="BodyTemplatePart"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<BodyTemplatePart> builder)
		{
			builder.ToTable("BodyTemplateParts", "GameRules").
				HasComment("Части шаблона тела");

			builder.Property(x => x.BodyTemplateId)
				.HasColumnName("BodyTemplateId")
				.HasComment("Айди шаблона тела")
				.IsRequired();

			builder.HasOne(x => x.BodyTemplate)
				.WithMany(x => x.BodyTemplateParts)
				.HasForeignKey(x => x.BodyTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ArmorTemplates)
				.WithMany(x => x.BodyTemplateParts);

			var bodyTemplateNavigation = builder.Metadata.FindNavigation(nameof(BodyTemplatePart.BodyTemplate));
			bodyTemplateNavigation.SetField(BodyTemplatePart.BodyTemplateField);
			bodyTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
