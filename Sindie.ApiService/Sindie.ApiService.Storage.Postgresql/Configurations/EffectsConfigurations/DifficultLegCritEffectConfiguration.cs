using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DifficultLegCritEffect"/>
	/// </summary>
	public class DifficultLegCritEffectConfiguration : HierarchyConfiguration<DifficultLegCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DifficultLegCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DifficultLegCritEffect> builder)
		{
			builder.ToTable("DifficultLegCritEffects", "Effects")
				.HasComment("Эффекты открытого перелома ноги");
		}
	}
}
