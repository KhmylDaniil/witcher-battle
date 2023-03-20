using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="ComplexHead1CritEffect"/>
	/// </summary>
	public class ComplexHead1CritEffectConfiguration : HierarchyConfiguration<ComplexHead1CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="ComplexHead1CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<ComplexHead1CritEffect> builder)
		{
			builder.ToTable("ComplexHead1CritEffects", "Effects")
				.HasComment("Эффекты выбитых зубов");
		}
	}
}
