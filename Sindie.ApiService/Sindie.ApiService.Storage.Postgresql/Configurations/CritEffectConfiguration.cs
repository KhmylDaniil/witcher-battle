using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;
using static Sindie.ApiService.Core.BaseData.Enums;
using System;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="CritEffect"/>
	/// </summary>
	public class CritEffectConfiguration : HierarchyConfiguration<CritEffect>
	{
		/// <summary>
		/// Конфигурация для <see cref="CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<CritEffect> builder)
		{
			builder.ToTable("CritEffects", "Battles")
				.HasComment("Критические эффекты");

			builder.Property(x => x.Severity)
				.HasColumnName("Severity")
				.HasComment("Тяжесть критического эффекта")
				.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<Severity>(v))
				.IsRequired();

			builder.Property(x => x.CreaturePartId)
				.HasColumnName("CreaturePartId")
				.HasComment("Айди части тела")
				.IsRequired();

			builder.Property(x => x.BodyPartLocation)
				.HasColumnName("BodyPartLocation")
				.HasComment("Тип части тела")
				.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<BodyPartType>(v))
				.IsRequired();

			var creaturePartNavigation = builder.Metadata.FindNavigation(nameof(CritEffect.CreaturePart));
			creaturePartNavigation.SetField(CritEffect.CreaturePartField);
			creaturePartNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
