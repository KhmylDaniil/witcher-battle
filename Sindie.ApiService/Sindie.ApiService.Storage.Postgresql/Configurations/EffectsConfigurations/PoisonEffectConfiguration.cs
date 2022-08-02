using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="PoisonEffect"/>
	/// </summary>
	public class PoisonEffectConfiguration : HierarchyConfiguration<PoisonEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="PoisonEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<PoisonEffect> builder)
		{
			builder.ToTable("PoisonEffects", "Effects")
				.HasComment("Эффекты отравления");
		}
	}
}
