using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities.Effects;

namespace Witcher.Storage.MySql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DeadlyHead1CritEffect"/>
	/// </summary>
	public class DeadlyHead1CritEffectConfiguration : HierarchyConfiguration<DeadlyHead1CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DeadlyHead1CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DeadlyHead1CritEffect> builder)
		{
			builder.ToTable("DeadlyHead1CritEffects", "Effects")
				.HasComment("Эффекты повреждения глаза");
		}
	}
}
