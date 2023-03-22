using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.MySql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=DeadlyWingCritEffect"/>
	/// </summary>
	public class DeadlyWingCritEffectConfiguration : HierarchyConfiguration<DeadlyWingCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DeadlyWingCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DeadlyWingCritEffect> builder)
		{
			builder.ToTable("DeadlyWingCritEffects", "Effects")
				.HasComment("Эффекты потери крыла");

			builder.Property(x => x.SpeedModifier)
			.HasColumnName("SpeedModifier")
			.HasComment("Модификатор скорости")
			.IsRequired();

			builder.Property(x => x.DodgeModifier)
			.HasColumnName("DodgeModifier")
			.HasComment("Модификатор уклонения")
			.IsRequired();

			builder.Property(x => x.AthleticsModifier)
			.HasColumnName("AthleticsModifier")
			.HasComment("Модификатор атлетики")
			.IsRequired();

			builder.Property(x => x.PenaltyApplied)
			.HasColumnName("PenaltyApplied")
			.HasComment("Пенальти применено")
			.IsRequired();
		}
	}
}
