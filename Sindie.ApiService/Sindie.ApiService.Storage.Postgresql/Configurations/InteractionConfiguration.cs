using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	public class InteractionConfiguration : EntityBaseConfiguration<Interaction>
	{
		public override void ConfigureChild(EntityTypeBuilder<Interaction> builder)
		{
			builder.ToTable("Interactions", "InteractionRules")
				.HasComment("Взаимодействия");

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(x => x.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла");

			builder.Property(x => x.TextFileId)
				.HasColumnName("TextFileId")
				.HasComment("Айди текстового файла");

			builder.Property(x => x.ScenarioReturnId)
				.HasColumnName("ScenarioReturnId")
				.HasComment("Айди сценария завершения взаимодействия");

			builder.Property(x => x.ScenarioVictoryId)
				.HasColumnName("ScenarioVictoryId")
				.HasComment("Айди сценария победы во взаимодействии");

			builder.Property(x => x.ScenarioLootId)
				.HasColumnName("ScenarioLootId")
				.HasComment("Айди сценария лута во взаимодействии");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название взаимодействия")
				.IsRequired();

			builder.HasIndex(x => x.Name)
				.IsUnique();

			builder.Property(x => x.Description)
				.HasColumnName("Description")
				.HasComment("Описание описание взаимодействия");

			builder.Property(x => x.CanGiveUp)
				.HasColumnName("CanGiveUp")
				.HasComment("Можно ли выйти из взаимодействия")
				.IsRequired();

			builder.Property(x => x.RoundLimit)
				.HasColumnName("RoundLimit")
				.HasComment("Максимальное количество раундов взаимодействия")
				.IsRequired();

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Interactions)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.Interaction)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.HasForeignKey<Interaction>(x => x.ImgFileId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.TextFile)
				.WithOne(x => x.Interaction)
				.HasPrincipalKey<TextFile>(x => x.Id)
				.HasForeignKey<Interaction>(x => x.TextFileId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.ScenarioReturn)
				.WithMany(x => x.ScenarioReturnInteractions)
				.HasForeignKey(x => x.ScenarioReturnId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.ScenarioVictory)
				.WithMany(x => x.ScenarioVictoryInteractions)
				.HasForeignKey(x => x.ScenarioVictoryId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.ScenarioLoot)
				.WithMany(x => x.ScenarioLootInteractions)
				.HasForeignKey(x => x.ScenarioLootId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.InteractionsRoles)
				.WithOne(x => x.Interaction)
				.HasForeignKey(x => x.InteractionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Parties)
				.WithOne(x => x.Interaction)
				.HasForeignKey(x => x.InteractionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Activities)
				.WithOne(x => x.Interaction)
				.HasForeignKey(x => x.InteractionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Actions)
				.WithOne(x => x.Interaction)
				.HasForeignKey(x => x.InteractionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Characteristics)
				.WithOne(x => x.Interaction)
				.HasForeignKey(x => x.InteractionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Interaction.Game));
			gameNavigation.SetField(Interaction.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(Interaction.ImgFile));
			imgFileNavigation.SetField(Interaction.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var textFileNavigation = builder.Metadata.FindNavigation(nameof(Interaction.TextFile));
			textFileNavigation.SetField(Interaction.TextFileField);
			textFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioReturnNavigation = builder.Metadata.FindNavigation(nameof(Interaction.ScenarioReturn));
			scenarioReturnNavigation.SetField(Interaction.ScenarioReturnField);
			scenarioReturnNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioVictoryNavigation = builder.Metadata.FindNavigation(nameof(Interaction.ScenarioVictory));
			scenarioVictoryNavigation.SetField(Interaction.ScenarioVictoryField);
			scenarioVictoryNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioLootNavigation = builder.Metadata.FindNavigation(nameof(Interaction.ScenarioLoot));
			scenarioLootNavigation.SetField(Interaction.ScenarioLootField);
			scenarioLootNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
