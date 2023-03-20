using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности игра
	/// </summary>
	public class GameConfiguration : EntityBaseConfiguration<Game>
	{
		/// <summary>
		/// Конфигурация для сущности игра
		/// </summary>
		/// <param name="builder">строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Game> builder)
		{
			builder.ToTable("Games", "BaseGame").
				HasComment("Игры");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.HasIndex(x => x.Name)
				.IsUnique();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание игры");

			builder.Property(r => r.AvatarId)
				.HasColumnName("AvatarId")
				.HasComment("Айди аватара игры");

			builder.HasMany(x => x.UserGames)
				.WithOne(x => x.Game)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ImgFiles)
				.WithMany(x => x.Games);

			builder.HasMany(x => x.TextFiles)
				.WithMany(x => x.Games);

			builder.HasOne(x => x.Avatar)
				.WithOne(x => x.Game)
				.HasForeignKey<Game>(x => x.AvatarId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.Battles)
				.WithOne(x => x.Game)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureTemplates)
				.WithOne(x => x.Game)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.BodyTemplates)
				.WithOne(x => x.Game)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Abilities)
				.WithOne(x => x.Game)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var avatarNavigation = builder.Metadata.FindNavigation(nameof(Game.Avatar));
			avatarNavigation.SetField(Game.AvatarField);
			avatarNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
