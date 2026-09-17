using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class CreatureTemplateConfiguration : EntityConfiguration<CreatureTemplate>
	{
		public override void Configure(EntityTypeBuilder<CreatureTemplate> builder)
		{
			base.Configure(builder);

			builder.ToTable("CreatureTemplate");

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.IsRequired();

			builder.Property(x => x.BodyTemplateId)
				.HasColumnName("BodyTemplateId")
				.IsRequired();

			builder.Property(x => x.CreatureType)
				.HasColumnName("CreatureType")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder.Property(x => x.Description)
				.HasColumnName("Description")
				.HasColumnType("varchar(500)")
				.IsRequired(false);

			builder.Property(x => x.ImageKey)
				.HasColumnName("ImageKey")
				.HasColumnType("varchar(500)")
				.IsRequired(false);

			builder.Property(x => x.HP).HasColumnName("HP").IsRequired();
			builder.Property(x => x.Sta).HasColumnName("Sta").IsRequired();
			builder.Property(x => x.Int).HasColumnName("Int").IsRequired();
			builder.Property(x => x.Ref).HasColumnName("Ref").IsRequired();
			builder.Property(x => x.Dex).HasColumnName("Dex").IsRequired();
			builder.Property(x => x.Body).HasColumnName("Body").IsRequired();
			builder.Property(x => x.Emp).HasColumnName("Emp").IsRequired();
			builder.Property(x => x.Cra).HasColumnName("Cra").IsRequired();
			builder.Property(x => x.Will).HasColumnName("Will").IsRequired();
			builder.Property(x => x.Speed).HasColumnName("Speed").IsRequired();
			builder.Property(x => x.Luck).HasColumnName("Luck").IsRequired();

			builder.Property(x => x.Skills)
				.HasColumnType("jsonb")
				.HasColumnName("Skills")
				.HasComment("Skills")
				.IsRequired();

			builder.Property(x => x.DamageTypeModifiers)
				.HasColumnType("jsonb")
				.HasColumnName("DamageTypeModifiers")
				.HasComment("DamageTypeModifiers")
				.IsRequired();

			builder.HasMany(x => x.Parts)
				.WithOne()
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Abilities)
				.WithOne()
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
