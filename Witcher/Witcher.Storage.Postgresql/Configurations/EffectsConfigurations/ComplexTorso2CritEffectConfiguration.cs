using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.MySql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="ComplexTorso2CritEffect"/>
	/// </summary>
	public class ComplexTorso2CritEffectConfiguration : HierarchyConfiguration<ComplexTorso2CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="ComplexTorso2CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<ComplexTorso2CritEffect> builder)
		{
			builder.Property(x => x.RoundCounter)
				.HasColumnName("RoundCounter")
				.HasComment("Счетчик раундов")
				.IsRequired();

			builder.ToTable("ComplexTorso2CritEffects", "Effects")
				.HasComment("Эффекты разрыва селезенки");
		}
	}
}
