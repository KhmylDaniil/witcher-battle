using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	public class PrerequisiteConfiguration : EntityBaseConfiguration<Prerequisite>
	{
		public override void ConfigureChild(EntityTypeBuilder<Prerequisite> builder)
		{
			builder.ToTable("Prerequisites", "GameRules").
				HasComment("Пререквизиты");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание");

			builder.Property(r => r.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла пререквизита");

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.Prerequisite)
				.HasForeignKey<Prerequisite>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.ScriptPrerequisites)
				.WithOne(x => x.Prerequisite)
				.HasForeignKey(x => x.PrerequisiteId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Prerequisite.ImgFile));
			gameNavigation.SetField(Prerequisite.ImgFileField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
