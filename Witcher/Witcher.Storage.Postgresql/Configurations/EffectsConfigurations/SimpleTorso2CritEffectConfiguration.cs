using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="SimpleTorso2CritEffect"/>
	/// </summary>
	public class Simpleorso2CritEffectConfiguration : HierarchyConfiguration<SimpleTorso2CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="SimpleTorso2CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<SimpleTorso2CritEffect> builder)
		{
			builder.ToTable("SimpleTorso2CritEffects", "Effects")
				.HasComment("Эффекты треснувших ребер");
		}
	}
}
