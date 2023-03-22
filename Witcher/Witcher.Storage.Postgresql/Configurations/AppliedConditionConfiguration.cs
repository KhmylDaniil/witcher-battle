using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using System;

namespace Witcher.Storage.MySql.Configurations
{
	public class AppliedConditionConfiguration : EntityBaseConfiguration<AppliedCondition>
	{
		public override void ConfigureChild(EntityTypeBuilder<AppliedCondition> builder)
		{
			builder.ToTable("AppliedConditions", "GameRules").
				HasComment("Применяемые состояния");

			builder.Property(r => r.AbilityId)
				.HasColumnName("AbilityId")
				.HasComment("Айди способности")
				.IsRequired();

			builder.Property(r => r.Condition)
				.HasColumnName("Condition")
				.HasComment("Тип состояния")
								.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<Condition>(v))
				.IsRequired();

			builder.Property(r => r.ApplyChance)
				.HasColumnName("ApplyChance")
				.HasComment("Шанс применения")
				.IsRequired();

			builder.HasOne(x => x.Ability)
				.WithMany(x => x.AppliedConditions)
				.HasForeignKey(x => x.AbilityId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var abilityNavigation = builder.Metadata.FindNavigation(nameof(AppliedCondition.Ability));
			abilityNavigation.SetField(AppliedCondition.AbilityField);
			abilityNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
