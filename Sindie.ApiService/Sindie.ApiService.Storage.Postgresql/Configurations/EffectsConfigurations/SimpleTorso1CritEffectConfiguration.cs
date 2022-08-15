using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="SimpleTorso1CritEffect"/>
	/// </summary>
	public class Simpleorso1CritEffectConfiguration : HierarchyConfiguration<SimpleTorso1CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="SimpleTorso1CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<SimpleTorso1CritEffect> builder)
		{
			builder.ToTable("SimpleTorso1CritEffects", "Effects")
				.HasComment("Эффекты инородного объекта");
		}
	}
}
