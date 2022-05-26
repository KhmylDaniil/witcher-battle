using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация области применения
	/// </summary>
	public class ApplicationAreaConfiguration : EntityBaseConfiguration<ApplicationArea>
	{
		/// <summary>
		/// Конфигурация области применения
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<ApplicationArea> builder)
		{
			builder.ToTable("ApplicationAreas", "InteractionRules")
				.HasComment("Области применения");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название области применения")
				.IsRequired();

			builder.Property(x => x.Description)
				.HasColumnName("Description")
				.HasComment("Описание описание области применения");

			builder.HasMany(x => x.Activities)
				.WithOne(x => x.ApplicationArea)
				.HasForeignKey(x => x.ApplicationAreaId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}