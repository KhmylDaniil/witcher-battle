using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="FireEffect"/>
	/// </summary>
	public class FireEffectConfiguration : HierarchyConfiguration<FireEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="FireEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<FireEffect> builder)
		{
			builder.ToTable("FireEffects", "Effects")
				.HasComment("Эффекты горения");
		}
	}
}
