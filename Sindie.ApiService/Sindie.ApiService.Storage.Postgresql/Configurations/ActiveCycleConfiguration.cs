using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация сущности цикл активности скрипта модификатора
	/// </summary>
	public class ActiveCycleConfiguration : EntityBaseConfiguration<ActiveCycle>
	{
		/// <summary>
		/// Конфигурация сущности цикл активности скрипта модификатора
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<ActiveCycle> builder)
		{
			builder.ToTable("ActiveCycles", "GameRules").
				HasComment("Скрипты модифкатора");

			builder.Property(x => x.ModifierScriptId)
				.HasColumnName("ModifierScriptId")
				.HasComment("Айди скрипта модификатора")
				.IsRequired();

			builder.Property(x => x.ActivationBeginning)
				.HasColumnName("ActivationBeginning")
				.HasComment("Начало цикла активности")
				.IsRequired();

			builder.Property(x => x.ActivationEnd)
				.HasColumnName("ActivationEnd")
				.HasComment("Конец цикла активности")
				.IsRequired();

			builder.HasOne(x => x.ModifierScript)
				.WithMany(x => x.ActiveCycles)
				.HasForeignKey(x => x.ModifierScriptId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var modifiersScriptNavigation = builder.Metadata.FindNavigation(nameof(ActiveCycle.ModifierScript));
			modifiersScriptNavigation.SetField(ActiveCycle.ModifierScriptField);
			modifiersScriptNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
