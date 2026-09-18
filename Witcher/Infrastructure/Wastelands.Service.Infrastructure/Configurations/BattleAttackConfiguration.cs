using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class BattleAttackConfiguration : EntityConfiguration<BattleAttack>
	{
		public override void Configure(EntityTypeBuilder<BattleAttack> builder)
		{
			base.Configure(builder);

			builder.ToTable("BattleAttack");

			builder.Property(x => x.BattleId).HasColumnName("BattleId").IsRequired();
			builder.Property(x => x.AttackerKind).HasColumnName("AttackerKind").IsRequired();
			builder.Property(x => x.AttackerId).HasColumnName("AttackerId").IsRequired();
			builder.Property(x => x.AbilityId).HasColumnName("AbilityId").IsRequired();
			builder.Property(x => x.AttacksAllowed).HasColumnName("AttacksAllowed").IsRequired();
			builder.Property(x => x.AttacksUsed).HasColumnName("AttacksUsed").IsRequired();

			builder.Property(x => x.DefenderKind).HasColumnName("DefenderKind").IsRequired();
			builder.Property(x => x.DefenderId).HasColumnName("DefenderId").IsRequired();
			builder.Property(x => x.TargetedCreaturePartId).HasColumnName("TargetedCreaturePartId").IsRequired(false);
			builder.Property(x => x.AttackRoll).HasColumnName("AttackRoll").IsRequired(false);
			builder.Property(x => x.AttackerConfirmed).HasColumnName("AttackerConfirmed").IsRequired();
			builder.Property(x => x.DefensiveSkill).HasColumnName("DefensiveSkill").IsRequired(false);
			builder.Property(x => x.DefenseRoll).HasColumnName("DefenseRoll").IsRequired(false);
			builder.Property(x => x.DefenderConfirmed).HasColumnName("DefenderConfirmed").IsRequired();

			builder.Property(x => x.Phase).HasColumnName("Phase").IsRequired();
			builder.Property(x => x.LastHitSucceeded).HasColumnName("LastHitSucceeded").IsRequired(false);
			builder.Property(x => x.ResolvedCreaturePartId).HasColumnName("ResolvedCreaturePartId").IsRequired(false);
			builder.Property(x => x.DamageRoll).HasColumnName("DamageRoll").IsRequired(false);
		}
	}
}
