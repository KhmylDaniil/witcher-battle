using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.MySql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=DeadEffect"/>
	/// </summary>
	public class DeadEffectConfiguration : HierarchyConfiguration<DeadEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DeadEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DeadEffect> builder)
		{
			builder.ToTable("DeadEffects", "Effects")
				.HasComment("Эффекты смерти");
		}
	}
}
