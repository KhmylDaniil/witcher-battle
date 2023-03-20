using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DifficultTorso1CritEffect"/>
	/// </summary>
	public class DifficultTorso1CritEffectConfiguration : HierarchyConfiguration<DifficultTorso1CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DifficultTorso1CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DifficultTorso1CritEffect> builder)
		{
			builder.ToTable("DifficultTorso1CritEffects", "Effects")
				.HasComment("Эффекты сосущей раны грудной клетки");
		}
	}
}
