using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для персонажа
	/// </summary>
	public class CharacterConfiguration : EntityBaseConfiguration<Character>
	{
		/// <summary>
		/// Конфигурация для персонажа
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Character> builder)
		{
			builder.ToTable("Characters", "GameInstance")
				.HasComment("Персонажи");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Имя персонажа")
				.IsRequired();

			builder.Property(x => x.InstanceId)
				.HasColumnName("InstanceId")
				.HasComment("Айди экземпляра игры")
				.IsRequired();

			builder.Property(x => x.CharacterTemplateId)
				.HasColumnName("CharacterTemplateId")
				.HasComment("Айди шаблона персонажа");

			builder.Property(x => x.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла");

			builder.Property(x => x.TextFileId)
				.HasColumnName("TextFileId")
				.HasComment("Айди текстового файла");

			builder.Property(x => x.BagId)
				.HasColumnName("BagId")
				.HasComment("Айди сумки");

			builder.Property(x => x.UserGameActivatedId)
				.HasColumnName("UserGameActivatedId")
				.HasComment("Айди активировашего персонажа пользователя");

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание персонажа");

			builder.Property(x => x.ActivationTime)
				.HasColumnName("ActivationTime")
				.HasComment("Время активации персонажа");

			builder.HasOne(x => x.Instance)
				.WithMany(x => x.Characters)
				.HasForeignKey(x => x.InstanceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.CharacterTemplate)
				.WithMany(x => x.Characters)
				.HasForeignKey(x => x.CharacterTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.Character)
				.HasForeignKey<Character>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.TextFile)
				.WithOne(x => x.Character)
				.HasForeignKey<Character>(x => x.TextFileId)
				.HasPrincipalKey<TextFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.UserGameActivated)
				.WithOne(x => x.ActivateCharacter)
				.HasForeignKey<Character>(x => x.UserGameActivatedId)
				.HasPrincipalKey<UserGameCharacter>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Bag)
				.WithOne()
				.HasForeignKey<Bag>(x => x.CharacterId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.CharacterParameters)
				.WithOne(x => x.Character)
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CharacterModifiers)
				.WithOne(x => x.Character)
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Bodies)
				.WithOne(x => x.Character)
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.UserGameCharacters)
				.WithOne(x => x.Character)
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.NotificationTradeRequests)
				.WithOne(x => x.ReceiveCharacter)
				.HasForeignKey(x => x.ReceiveCharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			var instanceNavigation = builder.Metadata.FindNavigation(nameof(Character.Instance));
			instanceNavigation.SetField(Character.InstanceField);
			instanceNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(Character.ImgFile));
			imgFileNavigation.SetField(Character.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var textFileNavigation = builder.Metadata.FindNavigation(nameof(Character.TextFile));
			textFileNavigation.SetField(Character.TextFileField);
			textFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var characterTemplateFileNavigation = builder.Metadata.FindNavigation(nameof(Character.CharacterTemplate));
			characterTemplateFileNavigation.SetField(Character.CharacterTemplateField);
			characterTemplateFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var bagNavigation = builder.Metadata.FindNavigation(nameof(Character.Bag));
			bagNavigation.SetField(Character.BagField);
			bagNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var userGameActivatedNavigation = builder.Metadata.FindNavigation(nameof(Character.UserGameActivated));
			userGameActivatedNavigation.SetField(Character.UserGameActivatedField);
			userGameActivatedNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
