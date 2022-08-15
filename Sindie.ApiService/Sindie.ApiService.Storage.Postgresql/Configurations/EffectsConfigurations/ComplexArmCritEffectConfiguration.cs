using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="ComplexArmCritEffect"/>
	/// </summary>
	public class ComplexArmCritEffectConfiguration : HierarchyConfiguration<ComplexArmCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="ComplexArmCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<ComplexArmCritEffect> builder)
		{
			builder.ToTable("ComplexArmCritEffects", "Effects")
				.HasComment("Эффекты перелома руки");

			builder.Property(x => x.PenaltyApplied)
			.HasColumnName("PenaltyApplied")
			.HasComment("Пенальти применено")
			.IsRequired();
		}
	}
}
