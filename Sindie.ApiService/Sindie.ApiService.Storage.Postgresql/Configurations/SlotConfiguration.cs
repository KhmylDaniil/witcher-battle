using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности слот
	/// </summary>
	public class SlotConfiguration : HierarchyConfiguration<Slot>
	{
		/// <summary>
		/// Конфигурация для сущности слот
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Slot> builder)
		{
			builder.ToTable("Slots", "GameRules").
				HasComment("Слоты");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Slots)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Items)
				.WithOne(x => x.Slot)
				.HasForeignKey(x => x.SlotId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CharacterTemplateSlots)
				.WithOne(x => x.Slot)
				.HasForeignKey(x => x.SlotId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Bodies)
				.WithOne(x => x.Slot)
				.HasForeignKey(x => x.SlotId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Slot.Game));
			gameNavigation.SetField(Slot.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
