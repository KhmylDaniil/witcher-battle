using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.MySql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="StunEffect"/>
	/// </summary>
	public class StunEffectConfiguration : HierarchyConfiguration<StunEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="StunEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<StunEffect> builder)
		{
			builder.ToTable("StunEffects", "Effects")
				.HasComment("Эффекты дезориентации");
		}
	}
}
