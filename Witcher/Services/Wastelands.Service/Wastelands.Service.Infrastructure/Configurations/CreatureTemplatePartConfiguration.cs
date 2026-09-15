using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class CreatureTemplatePartConfiguration : EntityConfiguration<CreatureTemplatePart>
	{
		public override void Configure(EntityTypeBuilder<CreatureTemplatePart> builder)
		{
			base.Configure(builder);

			builder.ToTable("CreatureTemplatePart");

			builder.Property(x => x.CreatureTemplateId)
				.HasColumnName("CreatureTemplateId")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder.Property(x => x.BodyPartType)
				.HasColumnName("BodyPartType")
				.IsRequired();

			builder.Property(x => x.DamageModifier)
				.HasColumnName("DamageModifier")
				.IsRequired();

			builder.Property(x => x.HitPenalty)
				.HasColumnName("HitPenalty")
				.IsRequired();

			builder.Property(x => x.MinToHit)
				.HasColumnName("MinToHit")
				.IsRequired();

			builder.Property(x => x.MaxToHit)
				.HasColumnName("MaxToHit")
				.IsRequired();

			builder.Property(x => x.Armor)
				.HasColumnName("Armor")
				.IsRequired();
		}
	}
}
