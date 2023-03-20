using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DifficultLegCritEffect"/>
	/// </summary>
	public class DifficultLegCritEffectConfiguration : HierarchyConfiguration<DifficultLegCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DifficultLegCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DifficultLegCritEffect> builder)
		{
			builder.ToTable("DifficultLegCritEffects", "Effects")
				.HasComment("Эффекты открытого перелома ноги");

			builder.Property(x => x.SpeedModifier)
			.HasColumnName("SpeedModifier")
			.HasComment("Модификатор скорости")
			.IsRequired();

			builder.Property(x => x.AfterTreatSpeedModifier)
			.HasColumnName("AfterTreatSpeedModifier")
			.HasComment("Модификатор скорости после стабилизации")
			.IsRequired();

			builder.Property(x => x.DodgeModifier)
			.HasColumnName("DodgeModifier")
			.HasComment("Модификатор уклонения")
			.IsRequired();

			builder.Property(x => x.AfterTreatDodgeModifier)
			.HasColumnName("AfterTreatDodgeModifier")
			.HasComment("Модификатор уклонения после стабилизации")
			.IsRequired();

			builder.Property(x => x.AthleticsModifier)
			.HasColumnName("AthleticsModifier")
			.HasComment("Модификатор атлетики")
			.IsRequired();

			builder.Property(x => x.AfterTreatAthleticsModifier)
			.HasColumnName("AfterTreatAthleticsModifier")
			.HasComment("Модификатор атлетики после стабилизации")
			.IsRequired();

			builder.Property(x => x.PenaltyApplied)
			.HasColumnName("PenaltyApplied")
			.HasComment("Пенальти применено")
			.IsRequired();
		}
	}
}
