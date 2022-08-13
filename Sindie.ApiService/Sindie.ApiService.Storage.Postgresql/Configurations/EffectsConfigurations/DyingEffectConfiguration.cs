using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=DyingEffect"/>
	/// </summary>
	public class DyingEffectConfiguration : HierarchyConfiguration<DyingEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DyingEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DyingEffect> builder)
		{
			builder.ToTable("DyingEffects", "Effects")
				.HasComment("Эффекты при смерти");
		}
	}
}
