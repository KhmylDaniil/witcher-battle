using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация экземпляров игры
	/// </summary>
	public class InstanceConfiguration : EntityBaseConfiguration<Instance>
	{
		/// <summary>
		/// Конфигурация экземпляров игры
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Instance> builder)
		{
			builder.ToTable("Instances", "GameInstance")
				.HasComment("Экземпляры");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название экземпляра")
				.IsRequired();

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(r => r.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла");

			builder.Property(x => x.UserGameActivatedId)
				.HasColumnName("UserGameActivatedId")
				.HasComment("Айди активировавшего игру пользователя");

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание экземпляра");

			builder.Property(x => x.ActivationTime)
				.HasColumnName("ActivationTime")
				.HasComment("Время активации экземпляра");

			builder.Property(x => x.DateOfGame)
				.HasColumnName("DateOfGame")
				.HasComment("Дата проведения игры");

			builder.Property(x => x.StoryAboutRules)
				.HasColumnName("StoryAboutRules")
				.HasComment("Описание правил игры");

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Instances)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.Instance)
				.HasForeignKey<Instance>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.UserGameActivated)
				.WithMany(x => x.Instances)
				.HasForeignKey(x => x.UserGameActivatedId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.Characters)
				.WithOne(x => x.Instance)
				.HasForeignKey(x => x.InstanceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Bags)
				.WithOne(x => x.Instance)
				.HasForeignKey(x => x.InstanceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Creatures)
				.WithOne(x => x.Instance)
				.HasForeignKey(x => x.InstanceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureBodies)
				.WithOne(x => x.Instance)
				.HasForeignKey(x => x.InstanceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(Instance.ImgFile));
			imgFileNavigation.SetField(Instance.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Instance.Game));
			gameNavigation.SetField(Instance.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var userGameActivatedNavigation = builder.Metadata.FindNavigation(nameof(Instance.UserGameActivated));
			userGameActivatedNavigation.SetField(Instance.UserGameActivatedField);
			userGameActivatedNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
