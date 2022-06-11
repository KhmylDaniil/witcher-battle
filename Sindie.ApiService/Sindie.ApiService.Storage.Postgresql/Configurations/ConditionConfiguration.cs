using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="Condition"/>
	/// </summary>
	public class ConditionConfiguration: EntityBaseConfiguration<Condition>
	{
		/// <summary>
		/// Конфигурация для <see cref="Condition"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<Condition> builder)
		{
			builder.ToTable("Conditions", "GameRules").
				HasComment("Состояния");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название состояния")
				.IsRequired();

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.HasMany(x => x.Creatures)
				.WithMany(x => x.Conditions);

			builder.HasMany(x => x.AppliedConditions)
				.WithOne(x => x.Condition)
				.HasForeignKey(x => x.ConditionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Conditions)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Condition.Game));
			gameNavigation.SetField(Condition.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
