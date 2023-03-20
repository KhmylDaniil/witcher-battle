using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="BleedEffect"/>
	/// </summary>
	public class BleedEffectConfiguration : HierarchyConfiguration<BleedEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="BleedEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<BleedEffect> builder)
		{
			builder.ToTable("BleedEffects", "Effects")
				.HasComment("Эффекты кровотечения");
		}
	}
}
