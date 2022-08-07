using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="SimpleLegCritEffect"/>
	/// </summary>
	public class SimpleLegCritEffectConfiguration : HierarchyConfiguration<SimpleLegCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="SimpleLegCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<SimpleLegCritEffect> builder)
		{
			builder.ToTable("SimpleLegCritEffects", "Effects")
				.HasComment("Эффекты вывиха ноги");
		}
	}
}
