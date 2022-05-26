using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности статья
	/// </summary>
	public class ArticleConfiguration : EntityBaseConfiguration<Article>
	{
		/// <summary>
		/// Конфигурация для сущности статья
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Article> builder)
		{
			builder.ToTable("Articles", "System")
				.HasComment("Статьи");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.Property(r => r.Text)
				.HasColumnName("Text")
				.HasComment("Текст")
				.IsRequired();

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры"); ;

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Articles)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

		}
	}
}
