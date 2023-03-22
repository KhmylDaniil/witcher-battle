using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.MySql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="SufflocationEffect"/>
	/// </summary>
	public class SufflocationEffectConfiguration : HierarchyConfiguration<SufflocationEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="SufflocationEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<SufflocationEffect> builder)
		{
			builder.ToTable("SufflocationEffects", "Effects")
				.HasComment("Эффекты удушья");
		}
	}
}
