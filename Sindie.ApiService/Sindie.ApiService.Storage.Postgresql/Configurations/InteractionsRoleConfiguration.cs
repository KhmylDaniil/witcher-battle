using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	public class InteractionsRoleConfiguration : EntityBaseConfiguration<InteractionsRole>
	{
		public override void ConfigureChild(EntityTypeBuilder<InteractionsRole> builder)
		{
			builder.ToTable("InteractionsRoles", "InteractionRules")
				.HasComment("Роли во взаимодействии");

			builder.Property(x => x.InteractionId)
				.HasColumnName("InteractionId")
				.HasComment("Айди взаимодействия для роли")
				.IsRequired();

			builder.Property(x => x.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла для роли");

			builder.Property(x => x.TextFileId)
				.HasColumnName("TextFileId")
				.HasComment("Айди текстового файла для роли");

			builder.Property(x => x.ScenarioReturnId)
				.HasColumnName("ScenarioReturnId")
				.HasComment("Айди сценария завершения взаимодействия для роли");

			builder.Property(x => x.ScenarioPrerequisitesId)
				.HasColumnName("ScenarioPrerequisites")
				.HasComment("Сценарий пререквизитов к роли")
				.IsRequired();

			builder.Property(x => x.ScenarioCharacteristicsId)
				.HasColumnName("ScenarioCharacteristicsId")
				.HasComment("Сценарий характеристик роли")
				.IsRequired();

			builder.Property(x => x.ScenarioInitiativeId)
				.HasColumnName("ScenarioInitiativeId")
				.HasComment("Сценарий инициативы роли")
				.IsRequired();

			builder.Property(x => x.ScenarioVictoryId)
				.HasColumnName("ScenarioVictoryId")
				.HasComment("Айди сценария победы во взаимодействии для роли")
				.IsRequired();

			builder.Property(x => x.ScenarioLootId)
				.HasColumnName("ScenarioLootId")
				.HasComment("Айди сценария лута во взаимодействии для роли")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название роли")
				.IsRequired();

			builder.HasIndex(x => x.Name)
				.IsUnique();

			builder.Property(x => x.Description)
				.HasColumnName("Description")
				.HasComment("Описание роли");

			builder.HasOne(x => x.Interaction)
				.WithMany(x => x.InteractionsRoles)
				.HasForeignKey(x => x.InteractionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.InteractionsRole)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.HasForeignKey<InteractionsRole>(x => x.ImgFileId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.TextFile)
				.WithOne(x => x.InteractionsRole)
				.HasPrincipalKey<TextFile>(x => x.Id)
				.HasForeignKey<InteractionsRole>(x => x.TextFileId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.ScenarioReturn)
				.WithMany(x => x.ScenarioReturnRoles)
				.HasForeignKey(x => x.ScenarioReturnId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.ScenarioVictory)
				.WithMany(x => x.ScenarioVictoryRoles)
				.HasForeignKey(x => x.ScenarioVictoryId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ScenarioLoot)
				.WithMany(x => x.ScenarioLootRoles)
				.HasForeignKey(x => x.ScenarioLootId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ScenarioPrerequisites)
				.WithMany(x => x.ScenarioPrerequisitesRoles)
				.HasForeignKey(x => x.ScenarioPrerequisitesId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ScenarioCharacteristics)
				.WithMany(x => x.ScenarioCharacteristicsRoles)
				.HasForeignKey(x => x.ScenarioCharacteristicsId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ScenarioInitiative)
				.WithMany(x => x.ScenarioInitiativeRoles)
				.HasForeignKey(x => x.ScenarioInitiativeId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.PartyInteractionsRoles)
				.WithOne(x => x.InteractionsRole)
				.HasForeignKey(x => x.InteractionsRoleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.InteractionsRoleActivities)
				.WithOne(x => x.InteractionsRole)
				.HasForeignKey(x => x.InteractionsRoleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.InteractionItems)
				.WithOne(x => x.InteractionsRole)
				.HasForeignKey(x => x.InteractionsRoleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var interactionNavigation = builder.Metadata.FindNavigation(nameof(InteractionsRole.Interaction));
			interactionNavigation.SetField(InteractionsRole.InteractionField);
			interactionNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(InteractionsRole.ImgFile));
			imgFileNavigation.SetField(InteractionsRole.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var textFileNavigation = builder.Metadata.FindNavigation(nameof(InteractionsRole.TextFile));
			textFileNavigation.SetField(InteractionsRole.TextFileField);
			textFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioReturnNavigation = builder.Metadata.FindNavigation(nameof(InteractionsRole.ScenarioReturn));
			scenarioReturnNavigation.SetField(InteractionsRole.ScenarioReturnField);
			scenarioReturnNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioVictoryNavigation = builder.Metadata.FindNavigation(nameof(InteractionsRole.ScenarioVictory));
			scenarioVictoryNavigation.SetField(InteractionsRole.ScenarioVictoryField);
			scenarioVictoryNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioLootNavigation = builder.Metadata.FindNavigation(nameof(InteractionsRole.ScenarioLoot));
			scenarioLootNavigation.SetField(InteractionsRole.ScenarioLootField);
			scenarioLootNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioPrerequisitesNavigation = builder.Metadata.FindNavigation(nameof(InteractionsRole.ScenarioPrerequisites));
			scenarioPrerequisitesNavigation.SetField(InteractionsRole.ScenarioPrerequisitesField);
			scenarioPrerequisitesNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioCharacteristicsNavigation = builder.Metadata.FindNavigation(nameof(InteractionsRole.ScenarioCharacteristics));
			scenarioCharacteristicsNavigation.SetField(InteractionsRole.ScenarioCharacteristicsField);
			scenarioCharacteristicsNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioInitiativeNavigation = builder.Metadata.FindNavigation(nameof(InteractionsRole.ScenarioInitiative));
			scenarioInitiativeNavigation.SetField(InteractionsRole.ScenarioInitiativeField);
			scenarioInitiativeNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
