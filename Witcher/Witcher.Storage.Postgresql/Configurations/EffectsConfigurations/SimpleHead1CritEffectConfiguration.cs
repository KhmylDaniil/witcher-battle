using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="SimpleHead1CritEffect"/>
	/// </summary>
	public class SimpleHead1CritEffectConfiguration : HierarchyConfiguration<SimpleHead1CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="SimpleHead1CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<SimpleHead1CritEffect> builder)
		{
			builder.ToTable("SimpleHead1CritEffects", "Effects")
				.HasComment("Эффекты уродующего шрама");
		}
	}
}
