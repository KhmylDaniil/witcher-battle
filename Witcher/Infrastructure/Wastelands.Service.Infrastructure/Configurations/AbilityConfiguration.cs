using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class AbilityConfiguration : EntityConfiguration<Ability>
	{
		public override void Configure(EntityTypeBuilder<Ability> builder)
		{
			base.Configure(builder);

			builder.ToTable("Ability");

			builder.Property(x => x.CreatureTemplateId)
				.HasColumnName("CreatureTemplateId")
				.IsRequired(false);

			builder.Property(x => x.CharacterId)
				.HasColumnName("CharacterId")
				.IsRequired(false);

			builder.Property(x => x.EquippedItemId)
				.HasColumnName("EquippedItemId")
				.IsRequired(false);

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder.Property(x => x.AttackSkill)
				.HasColumnName("AttackSkill")
				.IsRequired();

			builder.Property(x => x.AttacksPerTurn)
				.HasColumnName("AttacksPerTurn")
				.IsRequired();

			builder.Property(x => x.DamageDiceCount)
				.HasColumnName("DamageDiceCount")
				.IsRequired();

			builder.Property(x => x.AttackModifier)
				.HasColumnName("AttackModifier")
				.IsRequired();

			builder.Property(x => x.DamageModifier)
				.HasColumnName("DamageModifier")
				.IsRequired();

			builder.Property(x => x.DamageType)
				.HasColumnName("DamageType")
				.IsRequired();

			builder.HasMany(x => x.AppliedConditions)
				.WithOne()
				.HasForeignKey(x => x.AbilityId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.DefensiveSkills)
				.WithOne()
				.HasForeignKey(x => x.AbilityId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
