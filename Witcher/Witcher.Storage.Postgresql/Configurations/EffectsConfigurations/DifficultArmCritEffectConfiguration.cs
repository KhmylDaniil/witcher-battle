using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DifficultArmCritEffect"/>
	/// </summary>
	public class DifficultArmCritEffectConfiguration : HierarchyConfiguration<DifficultArmCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DifficultArmCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DifficultArmCritEffect> builder)
		{
			builder.ToTable("DifficultArmCritEffects", "Effects")
				.HasComment("Эффекты открытого перелома руки");

			builder.Property(x => x.PenaltyApplied)
			.HasColumnName("PenaltyApplied")
			.HasComment("Пенальти применено")
			.IsRequired();
		}
	}
}
