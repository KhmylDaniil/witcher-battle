using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.MySql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=DeadlyArmCritEffect"/>
	/// </summary>
	public class DeadlyArmCritEffectConfiguration : HierarchyConfiguration<DeadlyArmCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DeadlyArmCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DeadlyArmCritEffect> builder)
		{
			builder.ToTable("DeadlyArmCritEffects", "Effects")
				.HasComment("Эффекты потери руки");

			builder.Property(x => x.PenaltyApplied)
			.HasColumnName("PenaltyApplied")
			.HasComment("Пенальти применено")
			.IsRequired();
		}
	}
}
