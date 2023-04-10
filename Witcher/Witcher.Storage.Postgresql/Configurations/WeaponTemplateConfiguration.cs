﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Storage.Postgresql.Configurations
{
	public class WeaponTemplateConfiguration : HierarchyConfiguration<WeaponTemplate>
	{
		public override void ConfigureChild(EntityTypeBuilder<WeaponTemplate> builder)
		{
			builder.ToTable("WeaponTeemplates", "Items").
				HasComment("Шаблоны оружия");

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

			builder.Property(r => r.Durability)
				.HasColumnName("Durability")
				.HasComment("Прочность")
				.IsRequired();

			builder.Property(r => r.Range)
				.HasColumnName("Range")
				.HasComment("Дальность");

			builder.OwnsMany(x => x.DefensiveSkills).
				Property(ds => ds.Skill)
				.HasColumnName("DefensiveSkill")
				.HasComment("Защитный навык")
				.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<Skill>(v))
				.IsRequired();

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
