using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=StaggeredEffect"/>
	/// </summary>
	public class StaggeredEffectConfiguration : HierarchyConfiguration<StaggeredEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="StaggeredEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<StaggeredEffect> builder)
		{
			builder.ToTable("StaggeredEffects", "Effects")
				.HasComment("Эффекты ошеломления");
		}
	}
}
