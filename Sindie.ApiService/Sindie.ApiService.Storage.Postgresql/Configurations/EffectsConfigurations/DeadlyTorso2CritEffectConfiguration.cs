using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=DeadlyTorso2CritEffect"/>
	/// </summary>
	public class DeadlyTorso2CritEffectConfiguration : HierarchyConfiguration<DeadlyTorso2CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DeadlyTorso2CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DeadlyTorso2CritEffect> builder)
		{
			builder.ToTable("DeadlyTorso2CritEffects", "Effects")
				.HasComment("Эффекты травмы сердца");

			builder.Property(x => x.SpeedModifier)
			.HasColumnName("SpeedModifier")
			.HasComment("Модификатор скорости")
			.IsRequired();

			builder.Property(x => x.AfterTreatSpeedModifier)
			.HasColumnName("AfterTreatSpeedModifier")
			.HasComment("Модификатор скорости после стабилизации")
			.IsRequired();

			builder.Property(x => x.StaModifier)
			.HasColumnName("StaModifier")
			.HasComment("Модификатор стамины")
			.IsRequired();

			builder.Property(x => x.AfterTreatStaModifier)
			.HasColumnName("AfterTreatStaModifier")
			.HasComment("Модификатор стамины после стабилизации")
			.IsRequired();

			builder.Property(x => x.BodyModifier)
			.HasColumnName("BodyModifier")
			.HasComment("Модификатор телосложения")
			.IsRequired();

			builder.Property(x => x.AfterTreatBodyModifier)
			.HasColumnName("AfterTreatBodyModifier")
			.HasComment("Модификатор телосложения после стабилизации")
			.IsRequired();
		}
	}
}
