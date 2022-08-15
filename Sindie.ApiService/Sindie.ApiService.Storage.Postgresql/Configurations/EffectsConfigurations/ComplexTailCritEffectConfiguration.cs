using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="ComplexTailCritEffect"/>
	/// </summary>
	public class ComplexTailCritEffectConfiguration : HierarchyConfiguration<ComplexTailCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="ComplexTailCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<ComplexTailCritEffect> builder)
		{
			builder.ToTable("ComplexTailCritEffects", "Effects")
				.HasComment("Эффекты перелома хвоста");

			builder.Property(x => x.PenaltyApplied)
			.HasColumnName("PenaltyApplied")
			.HasComment("Пенальти применено")
			.IsRequired();
		}
	}
}
