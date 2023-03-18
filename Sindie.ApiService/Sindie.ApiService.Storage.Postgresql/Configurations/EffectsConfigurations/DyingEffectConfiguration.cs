using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
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

			builder.Property(x => x.Counter)
			.HasColumnName("Counter")
			.HasComment("Модификатор сложности")
			.IsRequired();
		}
	}
}
