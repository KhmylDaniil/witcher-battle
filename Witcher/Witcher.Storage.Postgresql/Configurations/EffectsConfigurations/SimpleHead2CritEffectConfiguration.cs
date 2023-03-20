using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="SimpleHead2CritEffect"/>
	/// </summary>
	public class SimpleHead2CritEffectConfiguration: HierarchyConfiguration<SimpleHead2CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="SimpleHead2CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<SimpleHead2CritEffect> builder)
		{
			builder.ToTable("SimpleHead2CritEffects", "Effects")
				.HasComment("Эффекты треснувшей челюсти");
		}
	}
}
