using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
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

			builder.HasOne(x => x.Prerequisite)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<Prerequisite>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.CharacterTemplate)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<CharacterTemplate>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.ItemTemplate)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<ItemTemplate>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Game)
				.WithOne(x => x.Avatar)
				.HasForeignKey<Game>(x => x.AvatarId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Interaction)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<Interaction>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Party)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<Party>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.InteractionsRole)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<InteractionsRole>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Activity)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<Activity>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Instance)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<Instance>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Character)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<Character>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Bag)
				.WithOne(x => x.ImgFile)
				.HasForeignKey<Bag>(x => x.ImgFileId)
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