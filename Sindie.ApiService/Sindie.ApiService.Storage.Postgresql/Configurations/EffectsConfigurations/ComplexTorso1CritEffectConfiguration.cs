using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="ComplexTorso1CritEffect"/>
	/// </summary>
	public class ComplexTorso1CritEffectConfiguration : HierarchyConfiguration<ComplexTorso1CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="ComplexTorso1CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<ComplexTorso1CritEffect> builder)
		{
			builder.ToTable("ComplexTorso1CritEffects", "Effects")
				.HasComment("Эффекты сломанных ребер");
		}
	}
}
