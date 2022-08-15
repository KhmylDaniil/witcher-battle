using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=DeadlyTorso1CritEffect"/>
	/// </summary>
	public class DeadlyTorso1CritEffectConfiguration : HierarchyConfiguration<DeadlyTorso1CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DeadlyTorso1CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DeadlyTorso1CritEffect> builder)
		{
			builder.ToTable("DeadlyTorso1CritEffects", "Effects")
				.HasComment("Эффекты септического шока");

			builder.Property(x => x.StaModifier)
			.HasColumnName("StaModifier")
			.HasComment("Модификатор стамины")
			.IsRequired();

			builder.Property(x => x.AfterTreatStaModifier)
			.HasColumnName("AfterTreatStaModifier")
			.HasComment("Модификатор стамины после стабилизации")
			.IsRequired();
		}
	}
}
