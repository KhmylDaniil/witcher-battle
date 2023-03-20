using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="ComplexLegCritEffect"/>
	/// </summary>
	public class ComplexLegCritEffectConfiguration : HierarchyConfiguration<ComplexLegCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="ComplexLegCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<ComplexLegCritEffect> builder)
		{
			builder.ToTable("ComplexLegCritEffects", "Effects")
				.HasComment("Эффекты перелома ноги");

			builder.Property(x => x.PenaltyApplied)
			.HasColumnName("PenaltyApplied")
			.HasComment("Пенальти применено")
			.IsRequired();
		}
	}
}
