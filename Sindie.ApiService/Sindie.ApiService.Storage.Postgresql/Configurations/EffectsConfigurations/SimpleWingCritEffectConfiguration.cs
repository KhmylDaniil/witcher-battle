using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="SimpleWingCritEffect"/>
	/// </summary>
	public class SimpleWingCritEffectConfiguration : HierarchyConfiguration<SimpleWingCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="SimpleWingCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<SimpleWingCritEffect> builder)
		{
			builder.ToTable("SimpleWingCritEffects", "Effects")
				.HasComment("Эффекты вывиха крыла");
		}
	}
}
