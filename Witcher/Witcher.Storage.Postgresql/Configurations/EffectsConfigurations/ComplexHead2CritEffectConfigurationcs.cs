using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.MySql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="ComplexHead2CritEffect"/>
	/// </summary>
	public class ComplexHead2CritEffectConfiguration : HierarchyConfiguration<ComplexHead2CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="ComplexHead2CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<ComplexHead2CritEffect> builder)
		{
			builder.ToTable("ComplexHead2CritEffects", "Effects")
				.HasComment("Эффекты небольшой травмы головы");
		}
	}
}
