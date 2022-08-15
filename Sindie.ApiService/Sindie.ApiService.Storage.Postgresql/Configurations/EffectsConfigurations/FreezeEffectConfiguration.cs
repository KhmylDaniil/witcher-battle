using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="FreezeEffect"/>
	/// </summary>
	public class FreezeEffectConfiguration : HierarchyConfiguration<FreezeEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="FreezeEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<FreezeEffect> builder)
		{
			builder.ToTable("FreezeEffects", "Effects")
				.HasComment("Эффекты заморозки");
		}
	}
}
