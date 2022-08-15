using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities.Effects;

namespace Sindie.ApiService.Storage.Postgresql.Configurations.EffectsConfigurations
{
	/// <summary>
	/// Конфигурация <see cref="DifficultHead1CritEffect"/>
	/// </summary>
	public class DifficultHead1CritEffectConfiguration : HierarchyConfiguration<DifficultHead1CritEffect>
	{
		/// <summary>
		/// Конфигурация <see cref="DifficultHead1CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DifficultHead1CritEffect> builder)
		{
			builder.ToTable("DifficultHead1CritEffects", "Effects")
				.HasComment("Эффекты контузии");

			builder.Property(x => x.RoundCounter)
			.HasColumnName("RoundCounter")
			.HasComment("Счетчик раундов")
			.IsRequired();

			builder.Property(x => x.NextCheck)
			.HasColumnName("NextCheck")
			.HasComment("Раунд следующей проверки")
			.IsRequired();
		}
	}
}
