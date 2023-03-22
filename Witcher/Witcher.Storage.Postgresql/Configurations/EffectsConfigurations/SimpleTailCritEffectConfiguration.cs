using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.MySql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="SimpleTailCritEffect"/>
	/// </summary>
	public class SimpleTailCritEffectConfiguration : HierarchyConfiguration<SimpleTailCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="SimpleTailCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<SimpleTailCritEffect> builder)
		{
			builder.ToTable("SimpleTailCritEffects", "Effects")
				.HasComment("Эффекты вывиха крыла");

			builder.Property(x => x.PenaltyApplied)
			.HasColumnName("PenaltyApplied")
			.HasComment("Пенальти применено")
			.IsRequired();
		}
	}
}
