using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DifficultTailCritEffect"/>
	/// </summary>
	public class DifficultTailCritEffectConfiguration : HierarchyConfiguration<DifficultTailCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DifficultTailCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DifficultTailCritEffect> builder)
		{
			builder.ToTable("DifficultTailCritEffects", "Effects")
				.HasComment("Эффекты открытого перелома хвоста");

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
