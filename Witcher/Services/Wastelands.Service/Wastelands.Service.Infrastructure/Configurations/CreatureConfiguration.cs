using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class CreatureConfiguration : EntityConfiguration<Creature>
	{
		public override void Configure(EntityTypeBuilder<Creature> builder)
		{
			base.Configure(builder);

			builder.ToTable("Creature");

			builder.Property(x => x.BattleId)
				.HasColumnName("BattleId")
				.IsRequired();

			builder.Property(x => x.CreatureTemplateId)
				.HasColumnName("CreatureTemplateId")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder.Property(x => x.CreatureType)
				.HasColumnName("CreatureType")
				.IsRequired();

			builder.Property(x => x.MaxHP).HasColumnName("MaxHP").IsRequired();
			builder.Property(x => x.CurrentHP).HasColumnName("CurrentHP").IsRequired();
			builder.Property(x => x.MaxSta).HasColumnName("MaxSta").IsRequired();
			builder.Property(x => x.CurrentSta).HasColumnName("CurrentSta").IsRequired();
			builder.Property(x => x.Ref).HasColumnName("Ref").IsRequired();

			builder.Property(x => x.Initiative)
				.HasColumnName("Initiative")
				.IsRequired(false);

			builder.Property(x => x.AppliedConditions)
				.HasColumnType("jsonb")
				.HasColumnName("AppliedConditions")
				.HasComment("AppliedConditions")
				.IsRequired();
		}
	}
}
