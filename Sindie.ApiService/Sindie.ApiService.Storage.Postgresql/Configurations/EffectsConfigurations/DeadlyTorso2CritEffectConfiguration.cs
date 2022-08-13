using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=DeadlyTorso2CritEffect"/>
	/// </summary>
	public class DeadlyTorso2CritEffectConfiguration : HierarchyConfiguration<DeadlyTorso2CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DeadlyTorso2CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DeadlyTorso2CritEffect> builder)
		{
			builder.ToTable("DeadlyTorso2CritEffects", "Effects")
				.HasComment("Эффекты травмы сердца");
		}
	}
}
