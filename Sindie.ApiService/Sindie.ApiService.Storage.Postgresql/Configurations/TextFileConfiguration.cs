using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности текстовый файл
	/// </summary>
	public class TextFileConfiguration : EntityBaseConfiguration<TextFile>
	{
		/// <summary>
		/// Конфигурация для сущности текстовый файл
		/// </summary>
		/// <param name="builder">строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<TextFile> builder)
		{
			builder.ToTable("TextFiles", "System")
				.HasComment("Текстовые файлы");

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
			.WithMany(x => x.TextFiles);

			builder.HasMany(x => x.Users)
			.WithMany(x => x.TextFiles);

			builder.HasOne(x => x.Interaction)
				.WithOne(x => x.TextFile)
				.HasForeignKey<Interaction>(x => x.TextFileId)
				.HasPrincipalKey<TextFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Party)
				.WithOne(x => x.TextFile)
				.HasForeignKey<Party>(x => x.TextFileId)
				.HasPrincipalKey<TextFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.InteractionsRole)
				.WithOne(x => x.TextFile)
				.HasForeignKey<InteractionsRole>(x => x.TextFileId)
				.HasPrincipalKey<TextFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.Character)
				.WithOne(x => x.TextFile)
				.HasForeignKey<Character>(x => x.TextFileId)
				.HasPrincipalKey<TextFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);
		}
	}
}