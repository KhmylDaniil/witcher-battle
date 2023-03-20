using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DifficultWingCritEffect"/>
	/// </summary>
	public class DifficultWingCritEffectConfiguration : HierarchyConfiguration<DifficultWingCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DifficultWingCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DifficultWingCritEffect> builder)
		{
			builder.ToTable("DifficultWingCritEffects", "Effects")
				.HasComment("Эффекты открытого перелома крыла");

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
