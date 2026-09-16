using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class AbilityAppliedConditionConfiguration : EntityConfiguration<AbilityAppliedCondition>
	{
		public override void Configure(EntityTypeBuilder<AbilityAppliedCondition> builder)
		{
			base.Configure(builder);

			builder.ToTable("AbilityAppliedCondition");

			builder.Property(x => x.AbilityId)
				.HasColumnName("AbilityId")
				.IsRequired();

			builder.Property(x => x.Condition)
				.HasColumnName("Condition")
				.IsRequired();

			builder.Property(x => x.ApplyChance)
				.HasColumnName("ApplyChance")
				.IsRequired();
		}
	}
}
