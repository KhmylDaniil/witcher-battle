using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="SimpleArmCritEffect"/>
	/// </summary>
	public class SimpleArmCritEffectConfiguration : HierarchyConfiguration<SimpleArmCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="SimpleArmCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<SimpleArmCritEffect> builder)
		{
			builder.ToTable("SimpleArmCritEffects", "Effects")
				.HasComment("Эффекты вывиха руки");

			builder.Property(x => x.PenaltyApplied)
			.HasColumnName("PenaltyApplied")
			.HasComment("Пенальти применено")
			.IsRequired();
		}
	}
}
