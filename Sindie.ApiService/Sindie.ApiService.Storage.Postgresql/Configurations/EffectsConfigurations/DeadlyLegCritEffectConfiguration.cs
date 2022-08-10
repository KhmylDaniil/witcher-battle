using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=DeadlyLegCritEffect"/>
	/// </summary>
	public class DeadlyLegCritEffectConfiguration : HierarchyConfiguration<DeadlyLegCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DeadlyLegCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DeadlyLegCritEffect> builder)
		{
			builder.ToTable("DeadlyLegCritEffects", "Effects")
				.HasComment("Эффекты потери ноги");
		}
	}
}
