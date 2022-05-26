using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурации для сущности Скрипт модификатора
	/// </summary>
	public class ModifierScriptConfiguration : EntityBaseConfiguration<ModifierScript>
	{
		/// <summary>
		/// Конфигурации для сущности Скрипт модификатора
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<ModifierScript> builder)
		{
			builder.ToTable("ModifierScripts", "GameRules").
				HasComment("Скрипты модифкатора");

			builder.Property(r => r.EventId)
				.HasColumnName("EventId")
				.HasComment("Айди события");

			builder.Property(r => r.ModifierId)
				.HasColumnName("ModifierId")
				.HasComment("Айди модификатора")
				.IsRequired();

			builder.Property(r => r.ScriptId)
				.HasColumnName("ScriptId")
				.HasComment("Айди скрипта")
				.IsRequired();

			builder.HasOne(x => x.Script)
				.WithMany(x => x.ModifierScripts)
				.HasForeignKey(x => x.ScriptId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Event)
				.WithMany(x => x.ModifierScripts)
				.HasForeignKey(x => x.EventId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Modifier)
				.WithMany(x => x.ModifierScripts)
				.HasForeignKey(x => x.ModifierId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ActiveCycles)
				.WithOne(x => x.ModifierScript)
				.HasForeignKey(x => x.ModifierScriptId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigationScript = builder.Metadata.FindNavigation(nameof(ModifierScript.Script));
			gameNavigationScript.SetField(ModifierScript.ScriptField);
			gameNavigationScript.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameNavigationEvent = builder.Metadata.FindNavigation(nameof(ModifierScript.Event));
			gameNavigationEvent.SetField(ModifierScript.EventField);
			gameNavigationEvent.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameNavigationModifier = builder.Metadata.FindNavigation(nameof(ModifierScript.Modifier));
			gameNavigationModifier.SetField(ModifierScript.ModifierField);
			gameNavigationModifier.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
