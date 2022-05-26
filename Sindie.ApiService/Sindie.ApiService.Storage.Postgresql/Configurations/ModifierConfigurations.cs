using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности модификатор
	/// </summary>
	public class ModifierConfigurations : HierarchyConfiguration<Modifier>
	{
		/// <summary>
		/// Конфигурация для сущности модификатор
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Modifier> builder)
		{
			builder.ToTable("Modifiers", "GameRules").
				HasComment("Модификаторы");

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Modifiers)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ModifierScripts)
				.WithOne(x => x.Modifier)
				.HasForeignKey(x => x.ModifierId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CharacterTemplateModifiers)
				.WithOne(x => x.Modifier)
				.HasForeignKey(x => x.ModifierId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ItemTemplateModifiers)
				.WithOne(x => x.Modifier)
				.HasForeignKey(x => x.ModifierId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CharacterModifiers)
				.WithOne(x => x.Modifier)
				.HasForeignKey(x => x.ModifierId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Modifier.Game));
			gameNavigation.SetField(Modifier.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
