using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DifficultWingCritEffect"/>
	/// </summary>
	public class DifficultWingCritEffectConfiguration : HierarchyConfiguration<DifficultWingCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DifficultWingCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DifficultWingCritEffect> builder)
		{
			builder.ToTable("DifficultWingCritEffects", "Effects")
				.HasComment("Эффекты открытого перелома крыла");
		}
	}
}
