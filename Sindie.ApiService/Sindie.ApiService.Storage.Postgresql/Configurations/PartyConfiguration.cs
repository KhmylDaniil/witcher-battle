using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для стороны
	/// </summary>
	public class PartyConfiguration : EntityBaseConfiguration<Party>
	{
		/// <summary>
		/// Конфигурация для стороны
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Party> builder)
		{
			builder.ToTable("Parties", "InteractionRules")
				.HasComment("Стороны");

			builder.Property(x => x.InteractionId)
				.HasColumnName("InteractionId")
				.HasComment("Айди взаимодействия для стороны")
				.IsRequired();

			builder.Property(x => x.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла для стороны");

			builder.Property(x => x.TextFileId)
				.HasColumnName("TextFileId")
				.HasComment("Айди текстового файла для стороны");

			builder.Property(x => x.ScenarioReturnId)
				.HasColumnName("ScenarioReturnId")
				.HasComment("Айди сценария завершения взаимодействия для стороны");

			builder.Property(x => x.ScenarioVictoryId)
				.HasColumnName("ScenarioVictoryId")
				.HasComment("Айди сценария победы во взаимодействии для стороны");

			builder.Property(x => x.ScenarioLootId)
				.HasColumnName("ScenarioLootId")
				.HasComment("Айди сценария лута во взаимодействии для стороны");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название стороны")
				.IsRequired();

			builder.Property(x => x.Description)
				.HasColumnName("Description")
				.HasComment("Описание стороны");

			builder.Property(x => x.MinQuantityCharacters)
				.HasColumnName("MinQuantityCharacters")
				.HasComment("Минимальное количество персонажей")
				.IsRequired();

			builder.Property(x => x.MaxQuantityCharacters)
				.HasColumnName("MaxQuantityCharacters")
				.HasComment("Максимальное количество персонажей")
				.IsRequired();

			builder.HasOne(x => x.Interaction)
				.WithMany(x => x.Parties)
				.HasForeignKey(x => x.InteractionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.Party)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.HasForeignKey<Party>(x => x.ImgFileId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.TextFile)
				.WithOne(x => x.Party)
				.HasPrincipalKey<TextFile>(x => x.Id)
				.HasForeignKey<Party>(x => x.TextFileId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.ScenarioReturn)
				.WithMany(x => x.ScenarioReturnParties)
				.HasForeignKey(x => x.ScenarioReturnId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.ScenarioVictory)
				.WithMany(x => x.ScenarioVictoryParties)
				.HasForeignKey(x => x.ScenarioVictoryId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.ScenarioLoot)
				.WithMany(x => x.ScenarioLootParties)
				.HasForeignKey(x => x.ScenarioLootId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.PartyInteractionsRoles)
				.WithOne(x => x.Party)
				.HasForeignKey(x => x.PartyId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var interactionNavigation = builder.Metadata.FindNavigation(nameof(Party.Interaction));
			interactionNavigation.SetField(Party.InteractionField);
			interactionNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(Party.ImgFile));
			imgFileNavigation.SetField(Party.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var textFileNavigation = builder.Metadata.FindNavigation(nameof(Party.TextFile));
			textFileNavigation.SetField(Party.TextFileField);
			textFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioReturnNavigation = builder.Metadata.FindNavigation(nameof(Party.ScenarioReturn));
			scenarioReturnNavigation.SetField(Party.ScenarioReturnField);
			scenarioReturnNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioVictoryNavigation = builder.Metadata.FindNavigation(nameof(Party.ScenarioVictory));
			scenarioVictoryNavigation.SetField(Party.ScenarioVictoryField);
			scenarioVictoryNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioLootNavigation = builder.Metadata.FindNavigation(nameof(Party.ScenarioLoot));
			scenarioLootNavigation.SetField(Party.ScenarioLootField);
			scenarioLootNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
