using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class AbilityDefensiveSkillConfiguration : EntityConfiguration<AbilityDefensiveSkill>
	{
		public override void Configure(EntityTypeBuilder<AbilityDefensiveSkill> builder)
		{
			base.Configure(builder);

			builder.ToTable("AbilityDefensiveSkill");

			builder.Property(x => x.AbilityId)
				.HasColumnName("AbilityId")
				.IsRequired();

			builder.Property(x => x.Skill)
				.HasColumnName("Skill")
				.IsRequired();
		}
	}
}
