using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wastelands.EfDataAccess.Configurations;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Infrastructure.Configurations
{
	public class ItemConfiguration : EntityConfiguration<Item>
	{
		public override void Configure(EntityTypeBuilder<Item> builder)
		{
			base.Configure(builder);

			builder.ToTable("Item");

			builder.Property(x => x.CharacterId)
				.HasColumnName("CharacterId")
				.IsRequired();

			builder.Property(x => x.ItemTemplateId)
				.HasColumnName("ItemTemplateId")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasColumnType("varchar(50)")
				.IsRequired();

			builder.Property(x => x.Description)
				.HasColumnName("Description")
				.HasColumnType("varchar(500)")
				.IsRequired(false);

			builder.Property(x => x.ItemType)
				.HasColumnName("ItemType")
				.IsRequired();

			builder.Property(x => x.Weight)
				.HasColumnName("Weight")
				.IsRequired();

			builder.Property(x => x.Cost)
				.HasColumnName("Cost")
				.IsRequired();

			builder.Property(x => x.AttackSkill)
				.HasColumnName("AttackSkill")
				.IsRequired(false);

			builder.Property(x => x.IsMultiAttack)
				.HasColumnName("IsMultiAttack")
				.IsRequired(false);

			builder.Property(x => x.DamageDiceCount)
				.HasColumnName("DamageDiceCount")
				.IsRequired(false);

			builder.Property(x => x.AttackModifier)
				.HasColumnName("AttackModifier")
				.IsRequired(false);

			builder.Property(x => x.DamageModifier)
				.HasColumnName("DamageModifier")
				.IsRequired(false);

			builder.Property(x => x.DamageType)
				.HasColumnName("DamageType")
				.IsRequired(false);

			builder.Property(x => x.WeaponKind)
				.HasColumnName("WeaponKind")
				.IsRequired(false);

			builder.Property(x => x.AttackRange)
				.HasColumnName("AttackRange")
				.IsRequired(false);

			builder.Property(x => x.HandsRequired)
				.HasColumnName("HandsRequired")
				.IsRequired(false);

			builder.Property(x => x.Durability)
				.HasColumnName("Durability")
				.IsRequired(false);

			builder.Property(x => x.IsEquipped)
				.HasColumnName("IsEquipped")
				.IsRequired();

			builder.Property(x => x.DamageTypeModifiers)
				.HasColumnType("jsonb")
				.HasColumnName("DamageTypeModifiers")
				.HasComment("DamageTypeModifiers")
				.IsRequired();

			builder.HasMany(x => x.AppliedConditions)
				.WithOne()
				.HasForeignKey(x => x.ItemId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ArmorParts)
				.WithOne()
				.HasForeignKey(x => x.ItemId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
