using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.MySql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DifficultTorso2CritEffect"/>
	/// </summary>
	public class DifficultTorso2CritEffectConfiguration : HierarchyConfiguration<DifficultTorso2CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DifficultTorso2CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DifficultTorso2CritEffect> builder)
		{
			builder.ToTable("DifficultTorso2CritEffects", "Effects")
				.HasComment("Эффекты раны в живот");
		}
	}
}
