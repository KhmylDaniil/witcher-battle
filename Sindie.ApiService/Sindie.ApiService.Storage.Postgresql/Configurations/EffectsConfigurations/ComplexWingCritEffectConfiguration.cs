using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="ComplexWingCritEffect"/>
	/// </summary>
	public class ComplexWingCritEffectConfiguration : HierarchyConfiguration<ComplexWingCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="ComplexWingCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<ComplexWingCritEffect> builder)
		{
			builder.ToTable("ComplexWingCritEffects", "Effects")
				.HasComment("Эффекты перелома крыла");

			builder.Property(x => x.PenaltyApplied)
			.HasColumnName("PenaltyApplied")
			.HasComment("Пенальти применено")
			.IsRequired();
		}
	}
}
