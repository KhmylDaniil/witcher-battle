using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Storage.Postgresql.Configurations
{
	public class WeaponConfiguration : HierarchyConfiguration<Weapon>
	{
		public override void ConfigureChild(EntityTypeBuilder<Weapon> builder)
		{
			builder.ToTable("Weapons", "Items").
				HasComment("Оружие");

			builder.Property(r => r.AttackSkill)
				.HasColumnName("AttackSkill")
				.HasComment("Навык атаки")
				.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<Skill>(v))
				.IsRequired();

			builder.Property(r => r.DamageType)
				.HasColumnName("DamageType")
				.HasComment("Тип урона")
				.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<DamageType>(v))
				.IsRequired();

			builder.Property(r => r.AttackDiceQuantity)
				.HasColumnName("AttackDiceQuantity")
				.HasComment("Количество кубов атаки")
				.IsRequired();

			builder.Property(r => r.DamageModifier)
				.HasColumnName("DamageModifier")
				.HasComment("Модификатор атаки")
				.IsRequired();

			builder.Property(r => r.Accuracy)
				.HasColumnName("Accuracy")
				.HasComment("Точность атаки")
				.IsRequired();

			builder.OwnsMany(x => x.DefensiveSkills).
				Property(ds => ds.Skill)
				.HasColumnName("DefensiveSkill")
				.HasComment("Защитный навык")
				.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<Skill>(v))
				.IsRequired();

			builder.HasMany(x => x.EquippedByCharacter)
				.WithMany(x => x.EquippedWeapons)
				.UsingEntity(x => x.ToTable("EquippedWeapons", "Items"));

			builder.OwnsMany(x => x.AppliedConditions)
				.Property(ac => ac.Condition)
				.HasColumnName("Condition")
				.HasComment("Тип состояния")
					.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<Condition>(v))
				.IsRequired();

			builder.OwnsMany(x => x.AppliedConditions)
				.Property(ac => ac.ApplyChance)
				.HasColumnName("ApplyChance")
				.HasComment("Шанс применения")
				.IsRequired();

		}
	}
}
