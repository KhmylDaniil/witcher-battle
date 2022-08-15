using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=ProneEffect"/>
	/// </summary>
	public class ProneEffectConfiguration : HierarchyConfiguration<ProneEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="ProneEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<ProneEffect> builder)
		{
			builder.ToTable("ProneEffects", "Effects")
				.HasComment("Эффекты падения");
		}
	}
}
