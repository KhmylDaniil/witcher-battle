using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурации для Скрипта
	/// </summary>
	public class ScriptConfiguration : EntityBaseConfiguration<Script>
	{
		/// <summary>
		/// Конфигурации для Скрипта
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Script> builder)
		{
			builder.ToTable("Scripts", "GameRules").
				HasComment("Скрипты");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание");

			builder.Property(r => r.BodyScript)
				.HasColumnName("BodyScript")
				.HasComment("Тело скрипта")
				.IsRequired();

			builder.Property(r => r.IsValid)
				.HasColumnName("IsValid")
				.HasComment("Валидность скрипта")
				.IsRequired();

			builder.Property(r => r.ValidationText)
				.HasColumnName("ValidationText")
				.HasComment("Валидационный текст");

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Scripts)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Items)
				.WithOne(x => x.Script)
				.HasForeignKey(x => x.ScriptId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.ModifierScripts)
				.WithOne(x => x.Script)
				.HasForeignKey(x => x.ScriptId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ScriptPrerequisites)
				.WithOne(x => x.Script)
				.HasForeignKey(x => x.ScriptId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ScenarioReturnInteractions)
				.WithOne(x => x.ScenarioReturn)
				.HasForeignKey(x => x.ScenarioReturnId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.ScenarioVictoryInteractions)
				.WithOne(x => x.ScenarioVictory)
				.HasForeignKey(x => x.ScenarioVictoryId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.ScenarioLootInteractions)
				.WithOne(x => x.ScenarioLoot)
				.HasForeignKey(x => x.ScenarioLootId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.ScenarioReturnParties)
				.WithOne(x => x.ScenarioReturn)
				.HasForeignKey(x => x.ScenarioReturnId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.ScenarioVictoryParties)
				.WithOne(x => x.ScenarioVictory)
				.HasForeignKey(x => x.ScenarioVictoryId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.ScenarioLootParties)
				.WithOne(x => x.ScenarioLoot)
				.HasForeignKey(x => x.ScenarioLootId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.ScenarioReturnRoles)
				.WithOne(x => x.ScenarioReturn)
				.HasForeignKey(x => x.ScenarioReturnId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.ScenarioVictoryRoles)
				.WithOne(x => x.ScenarioVictory)
				.HasForeignKey(x => x.ScenarioVictoryId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ScenarioLootRoles)
				.WithOne(x => x.ScenarioLoot)
				.HasForeignKey(x => x.ScenarioLootId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ScenarioPrerequisitesRoles)
				.WithOne(x => x.ScenarioPrerequisites)
				.HasForeignKey(x => x.ScenarioPrerequisitesId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ScenarioCharacteristicsRoles)
				.WithOne(x => x.ScenarioCharacteristics)
				.HasForeignKey(x => x.ScenarioCharacteristicsId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ScenarioInitiativeRoles)
				.WithOne(x => x.ScenarioInitiative)
				.HasForeignKey(x => x.ScenarioInitiativeId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Action)
				.WithOne(x => x.ScenarioAction)
				.HasForeignKey<Action>(x => x.ScenarioActionId)
				.HasPrincipalKey<Script>(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Script.Game));
			gameNavigation.SetField(Script.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
