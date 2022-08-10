using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
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
			builder.ToTable("DifficultTorso1CritEffects", "Effects")
				.HasComment("Эффекты раны в живот");
		}
	}
}
