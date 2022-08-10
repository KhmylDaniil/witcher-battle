using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
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
		}
	}
}
