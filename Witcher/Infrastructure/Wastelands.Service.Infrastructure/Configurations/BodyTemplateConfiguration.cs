using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class BodyTemplateConfiguration : EntityConfiguration<BodyTemplate>
	{
		public override void Configure(EntityTypeBuilder<BodyTemplate> builder)
		{
			base.Configure(builder);

			builder.ToTable("BodyTemplate");

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder.Property(x => x.Description)
				.HasColumnName("Description")
				.HasColumnType("varchar(500)")
				.IsRequired(false);

			builder.HasMany(x => x.Parts)
				.WithOne()
				.HasForeignKey(x => x.BodyTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureTemplates)
				.WithOne()
				.HasForeignKey(x => x.BodyTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
