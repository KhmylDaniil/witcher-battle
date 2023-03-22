using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.MySql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=DeadlyTailCritEffect"/>
	/// </summary>
	public class DeadlyTailCritEffectConfiguration : HierarchyConfiguration<DeadlyTailCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DeadlyTailCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DeadlyTailCritEffect> builder)
		{
			builder.ToTable("DeadlyTailCritEffects", "Effects")
				.HasComment("Эффекты потери хвоста");

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
