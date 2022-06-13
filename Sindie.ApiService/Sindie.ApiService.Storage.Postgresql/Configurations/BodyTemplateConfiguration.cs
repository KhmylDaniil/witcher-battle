using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{

	/// <summary>
	/// Конфигурация для <see cref="BodyTemplate"/>
	/// </summary>
	public class BodyTemplateConfiguration : EntityBaseConfiguration<BodyTemplate>
	{
		/// <summary>
		/// Конфигурация для <see cref="BodyTemplate"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<BodyTemplate> builder)
		{
			builder.ToTable("BodyTemplates", "GameRules").
				HasComment("Шаблоны тел");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название шаблона тела")
				.IsRequired();

			builder.Property(x => x.Description)
				.HasColumnName("Description")
				.HasComment("Описание");

			builder.HasOne(x => x.Game)
				.WithMany(x => x.BodyTemplates)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureTemplates)
				.WithOne(x => x.BodyTemplate)
				.HasForeignKey(x => x.BodyTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Creatures)
				.WithOne(x => x.BodyTemplate)
				.HasForeignKey(x => x.BodyTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.BodyTemplateParts)
				.WithOne(x => x.BodyTemplate)
				.HasForeignKey(x => x.BodyTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(BodyTemplate.Game));
			gameNavigation.SetField(BodyTemplate.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
