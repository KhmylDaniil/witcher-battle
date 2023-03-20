using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="SimpleLegCritEffect"/>
	/// </summary>
	public class SimpleLegCritEffectConfiguration : HierarchyConfiguration<SimpleLegCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="SimpleLegCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<SimpleLegCritEffect> builder)
		{
			builder.ToTable("SimpleLegCritEffects", "Effects")
				.HasComment("Эффекты вывиха ноги");

			builder.Property(x => x.PenaltyApplied)
			.HasColumnName("PenaltyApplied")
			.HasComment("Пенальти применено")
			.IsRequired();
		}
	}
}
