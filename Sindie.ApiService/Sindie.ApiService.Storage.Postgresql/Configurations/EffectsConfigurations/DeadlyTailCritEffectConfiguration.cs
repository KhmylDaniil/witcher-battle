using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref=DeadlyTailCritEffect"/>
	/// </summary>
	public class DeadlyTailCritEffectConfiguration : HierarchyConfiguration<DeadlyTailCritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DeadlyTailCritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DeadlyTailCritEffect> builder)
		{
			builder.ToTable("DeadlyTailCritEffects", "Effects")
				.HasComment("Эффекты потери хвоста");
		}
	}
}
