using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности графический файл
	/// </summary>
	public class ImgFileConfiguration : EntityBaseConfiguration<ImgFile>
	{
		/// <summary>
		/// Конфигурация для сущности графический файл
		/// </summary>
		/// <param name="builder">строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<ImgFile> builder)
		{
			builder.ToTable("ImgFiles", "System")
				.HasComment("Графические файлы");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("название файла")
				.IsRequired();

			builder.Property(r => r.Extension)
				.HasColumnName("Extension")
				.HasComment("Расширение файла")
				.IsRequired();

			builder.Property(r => r.Size)
				.HasColumnName("Size")
				.HasComment("размер файла")
				.IsRequired();

			builder.HasMany(x => x.Games)
				.WithMany(x => x.ImgFiles);

			builder.HasMany(x => x.Users)
				.WithMany(x => x.ImgFiles);

			builder.HasOne(x => x.UserAvatar)
				.WithOne(x => x.Avatar)
				.HasForeignKey<User>(x => x.AvatarId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Game)
				.WithOne(x => x.Avatar)
				.HasForeignKey<Game>(x => x.AvatarId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Battle)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<Battle>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.CreatureTemplate)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<CreatureTemplate>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Creature)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<Creature>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);
		}
	}
}