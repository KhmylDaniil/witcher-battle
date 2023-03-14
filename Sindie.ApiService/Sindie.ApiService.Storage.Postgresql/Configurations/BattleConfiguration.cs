using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация экземпляров игры
	/// </summary>
	public class BattleConfiguration : EntityBaseConfiguration<Battle>
	{
		/// <summary>
		/// Конфигурация экземпляров игры
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Battle> builder)
		{
			builder.ToTable("Battles", "Battles")
				.HasComment("Экземпляры");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название боя")
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
				.HasComment("Описание боя");

			builder.Property(x => x.NextInitiative)
				.HasColumnName("NextInitiative")
				.HasComment("Значение инициативы следующего существа");

			builder.Property(x => x.BattleLog)
				.HasColumnName("BattleLog")
				.HasComment("Журнал боя");

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Battles)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.Battle)
				.HasForeignKey<Battle>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.UserGameActivated)
				.WithMany(x => x.Instances)
				.HasForeignKey(x => x.UserGameActivatedId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.Creatures)
				.WithOne(x => x.Battle)
				.HasForeignKey(x => x.BattleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(Battle.ImgFile));
			imgFileNavigation.SetField(Battle.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Battle.Game));
			gameNavigation.SetField(Battle.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var userGameActivatedNavigation = builder.Metadata.FindNavigation(nameof(Battle.UserGameActivated));
			userGameActivatedNavigation.SetField(Battle.UserGameActivatedField);
			userGameActivatedNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
