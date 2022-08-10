using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DifficultTailCritEffect"/>
	/// </summary>
	public class DifficultTailCritEffectConfiguration : HierarchyConfiguration<DifficultTailCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DifficultTailCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DifficultTailCritEffect> builder)
		{
			builder.ToTable("DifficultTailCritEffects", "Effects")
				.HasComment("Эффекты открытого перелома хвоста");
		}
	}
}
