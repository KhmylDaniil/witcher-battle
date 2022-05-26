using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфиграция для сущности Шаблон предмета
	/// </summary>
	public class ItemTemplateConfiguration : EntityBaseConfiguration<ItemTemplate>
	{
		/// <summary>
		/// Конфиграция для сущности Шаблон предмета
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<ItemTemplate> builder)
		{
			builder.ToTable("ItemTemplates", "GameRules").
				HasComment("Шаблоны предметов");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(r => r.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание");

			builder.Property(r => r.Weight)
				.HasColumnName("Weight")
				.HasComment("Вес")
				.IsRequired();

			builder.Property(r => r.MaxQuantity)
				.HasColumnName("MaxQuantity")
				.HasComment("Максимальное количество в стаке")
				.IsRequired();

			builder.HasMany(x => x.Items)
				.WithOne(x => x.ItemTemplate)
				.HasForeignKey(x => x.ItemTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Game)
				.WithMany(x => x.ItemTemplates)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.ItemTemplate)
				.HasForeignKey<ItemTemplate>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.ItemTemplateModifiers)
				.WithOne(x => x.ItemTemplate)
				.HasForeignKey(x => x.ModifierId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigationGame = builder.Metadata.FindNavigation(nameof(ItemTemplate.Game));
			gameNavigationGame.SetField(ItemTemplate.GameField);
			gameNavigationGame.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameNavigationImgFile = builder.Metadata.FindNavigation(nameof(ItemTemplate.ImgFile));
			gameNavigationImgFile.SetField(ItemTemplate.ImgFileField);
			gameNavigationImgFile.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
