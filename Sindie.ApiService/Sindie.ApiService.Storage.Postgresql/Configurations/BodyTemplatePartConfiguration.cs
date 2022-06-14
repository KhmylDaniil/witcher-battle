using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="BodyTemplatePart"/>
	/// </summary>
	public class BodyTemplatePartConfiguration : EntityBaseConfiguration<BodyTemplatePart>
	{
		/// <summary>
		/// Конфигурация для <see cref="BodyTemplatePart"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<BodyTemplatePart> builder)
		{
			builder.ToTable("BodyTemplateParts", "GameRules")
				.HasComment("Части шаблона тела");

			builder.Property(r => r.BodyTemplateId)
				.HasColumnName("BodyTemplateId")
				.HasComment("Айди шаблона тела")
				.IsRequired();

			builder.Property(bp => bp.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.Property(bp => bp.DamageModifier)
				.HasColumnName("DamageModifer")
				.HasComment("Модификатор урона")
				.IsRequired();

			builder.Property(bp => bp.HitPenalty)
				.HasColumnName("HitPenalty")
				.HasComment("Пенальти за прицеливание")
				.IsRequired();

			builder.Property(bp => bp.MinToHit)
				.HasColumnName("MinToHit")
				.HasComment("Минимальное значение попадания")
				.IsRequired();

			builder.Property(bp => bp.MaxToHit)
				.HasColumnName("MaxToHit")
				.HasComment("Максимальное значение попадания")
				.IsRequired();

			builder.HasOne(x => x.BodyTemplate)
				.WithMany(x => x.BodyTemplateParts)
				.HasForeignKey(x => x.BodyTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var bodyTemplateNavigation = builder.Metadata.FindNavigation(nameof(BodyTemplatePart.BodyTemplate));
			bodyTemplateNavigation.SetField(BodyTemplatePart.BodyTemplateField);
			bodyTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
