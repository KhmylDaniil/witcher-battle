using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности Параметр
	/// </summary>
	public class ParameterConfiguration : HierarchyConfiguration<Parameter>
	{
		/// <summary>
		/// Конфигурация для сущности параметр
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Parameter> builder)
		{
			builder.ToTable("Parameters", "GameRules").
				HasComment("Параметры");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.OwnsOne(p => p.ParameterBounds, pb =>
			{
				pb.Property(p => p.MaxValueParameter)
				.HasColumnName("MaxValueParameters")
				.HasComment("Максимальные значения параметра")
				.IsRequired();
				pb.Property(p => p.MinValueParameter)
				.HasColumnName("MinValueParameters")
				.HasComment("Минимальные значения параметра")
				.IsRequired();
			});

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Parameters)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CharacterParameters)
				.WithOne(x => x.Parameter)
				.HasForeignKey(x => x.ParameterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureParameters)
				.WithOne(x => x.Parameter)
				.HasForeignKey(x => x.ParameterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureTemplateParameters)
				.WithOne(x => x.Parameter)
				.HasForeignKey(x => x.ParameterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Abilities)
				.WithOne(x => x.AttackParameter)
				.HasForeignKey(x => x.AttackParameterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigationGame = builder.Metadata.FindNavigation(nameof(Parameter.Game));
			gameNavigationGame.SetField(Parameter.GameField);
			gameNavigationGame.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
