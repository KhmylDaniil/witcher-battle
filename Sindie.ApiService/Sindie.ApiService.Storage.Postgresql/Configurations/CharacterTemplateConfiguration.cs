using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурации для Шаблона персонажа
	/// </summary>
	public class CharacterTemplateConfiguration : EntityBaseConfiguration<CharacterTemplate>
	{
		/// <summary>
		/// Конфигурации для Шаблона персонажа
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<CharacterTemplate> builder)
		{
			builder.ToTable("CharacterTemplates", "GameRules").
				HasComment("Шаблоны персонажей");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(r => r.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла");

			builder.Property(r => r.InterfaceId)
				.HasColumnName("InterfaceId")
				.HasComment("Айди интерфейса");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название шаблона")
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание шаблона");

			builder.HasOne(x => x.Game)
				.WithMany(x => x.CharacterTemplates)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CharacterTemplateModifiers)
				.WithOne(x => x.CharacterTemplate)
				.HasForeignKey(x => x.CharacterTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.CharacterTemplate)
				.HasForeignKey<CharacterTemplate>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Interface)
				.WithMany(x => x.CharacterTemplates)
				.HasForeignKey(x => x.InterfaceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.CharacterTemplateSlots)
				.WithOne(x => x.CharacterTemplate)
				.HasForeignKey(x => x.CharacterTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Characters)
				.WithOne(x => x.CharacterTemplate)
				.HasForeignKey(x => x.CharacterTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			var gameNavigationGame = builder.Metadata.FindNavigation(nameof(CharacterTemplate.Game));
			gameNavigationGame.SetField(CharacterTemplate.GameField);
			gameNavigationGame.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameNavigationImgFile = builder.Metadata.FindNavigation(nameof(CharacterTemplate.Interface));
			gameNavigationImgFile.SetField(CharacterTemplate.InterfaceField);
			gameNavigationImgFile.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameNavigationInterface = builder.Metadata.FindNavigation(nameof(CharacterTemplate.ImgFile));
			gameNavigationInterface.SetField(CharacterTemplate.ImgFileField);
			gameNavigationInterface.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
