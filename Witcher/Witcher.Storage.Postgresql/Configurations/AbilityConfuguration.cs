using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
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
				.IsRequired();

			builder.Property(r => r.DamageType)
				.HasColumnName("DamageType")
				.HasComment("Тип урона")
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

			builder.OwnsMany(x => x.DefensiveSkills).
				Property(ds => ds.Skill)
				.HasColumnName("DefensiveSkill")
				.HasComment("Защитный навык")
				.IsRequired();

			builder.OwnsMany(x => x.AppliedConditions)
				.Property(ac => ac.Condition)
				.HasColumnName("Condition")
				.HasComment("Тип состояния")
				.IsRequired();

			builder.OwnsMany(x => x.AppliedConditions)
				.Property(ac => ac.ApplyChance)
				.HasColumnName("ApplyChance")
				.HasComment("Шанс применения")
				.IsRequired();

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Abilities)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Creatures)
				.WithMany(x => x.Abilities)
				.UsingEntity(x => x.ToTable("CreatureAbilities", "Battles"));

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Ability.Game));
			gameNavigation.SetField(Ability.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
