using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="BleedingWoundEffect"/>
	/// </summary>
	public class BleedingWoundEffectConfiguration : HierarchyConfiguration<BleedingWoundEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="BleedingWoundEffect"/>
		/// </summary>
		public override void ConfigureChild(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BleedingWoundEffect> builder)
		{
			builder.ToTable("BleedingWoundEffects", "Effects")
				.HasComment("Эффекты кровавой раны");

			builder.Property(x => x.Severity)
			.HasColumnName("Severity")
			.HasComment("Тяжесть")
			.IsRequired();

			builder.Property(x => x.Damage)
			.HasColumnName("Damage")
			.HasComment("Урон")
			.IsRequired();
		}
	}
}
