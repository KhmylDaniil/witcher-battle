using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;
using static Sindie.ApiService.Core.BaseData.Enums;
using System;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="Ability"/>
	/// </summary>
	public class AbilityConfuguration : EntityBaseConfiguration<Ability>
	{
		/// <summary>
		/// Конфигурация для <see cref="Ability"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<Ability> builder)
		{
			builder.ToTable("Abilities", "GameRules").
				HasComment("Способности");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название способности")
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание способности");

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

			builder.Property(r => r.AttackSpeed)
				.HasColumnName("AttackSpeed")
				.HasComment("Скорость атаки")
				.IsRequired();

			builder.Property(r => r.Accuracy)
				.HasColumnName("Accuracy")
				.HasComment("Точность атаки")
				.IsRequired();

			builder.OwnsMany(x => x.DefensiveSkills)
				.HasKey(r => r.Id);

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Abilities)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Creatures)
				.WithMany(x => x.Abilities)
				.UsingEntity(x => x.ToTable("CreatureAbilities", "Battles"));

			builder.HasMany(x => x.AppliedConditions)
				.WithOne(x => x.Ability)
				.HasForeignKey(x => x.AbilityId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Ability.Game));
			gameNavigation.SetField(Ability.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
