using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			builder.Property(r => r.AttackSpeed)
				.HasColumnName("AttackSpeed")
				.HasComment("Скорость атаки")
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

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Abilities)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Creatures)
				.WithMany(x => x.Abilities)
				.UsingEntity(x => x.ToTable("CreatureAbilities", "GameInstance"));

			builder.HasMany(x => x.DamageTypes)
				.WithMany(x => x.Abilities)
				.UsingEntity(x => x.ToTable("AbilityDamageTypes", "GameRules"));

			builder.HasMany(x => x.AppliedConditions)
				.WithOne(x => x.Ability)
				.HasForeignKey(x => x.AbilityId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.AttackParameter)
				.WithMany(x => x.Abilities)
				.HasForeignKey(x => x.AttackParameterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Ability.Game));
			gameNavigation.SetField(Ability.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var parameterNavigation = builder.Metadata.FindNavigation(nameof(Ability.AttackParameter));
			parameterNavigation.SetField(Ability.ParameterField);
			parameterNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
