using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	public class AppliedConditionConfiguration : EntityBaseConfiguration<AppliedCondition>
	{
		public override void ConfigureChild(EntityTypeBuilder<AppliedCondition> builder)
		{
			builder.ToTable("AppliedConditions", "GameRules").
				HasComment("Применяемые состояния");

			builder.Property(r => r.AbilityId)
				.HasColumnName("AbilityId")
				.HasComment("Айди способнности")
				.IsRequired();

			builder.Property(r => r.ConditionId)
				.HasColumnName("ConditionId")
				.HasComment("Айди состояния")
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

			builder.HasOne(x => x.Condition)
				.WithMany(x => x.AppliedConditions)
				.HasForeignKey(x => x.ConditionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var abilityNavigation = builder.Metadata.FindNavigation(nameof(AppliedCondition.Ability));
			abilityNavigation.SetField(AppliedCondition.AbilityField);
			abilityNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var conditionNavigation = builder.Metadata.FindNavigation(nameof(AppliedCondition.Condition));
			conditionNavigation.SetField(AppliedCondition.ConditionField);
			conditionNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
