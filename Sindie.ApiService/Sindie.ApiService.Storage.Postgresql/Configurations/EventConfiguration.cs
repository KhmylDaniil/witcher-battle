using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности событие
	/// </summary>
	public class EventConfiguration : HierarchyConfiguration<Event>
	{
		/// <summary>
		/// Конфигурация для сущности событие
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Event> builder)
		{
			builder.ToTable("Events", "GameRules").
				HasComment("События");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(r => r.IsActive)
				.HasColumnName("IsActive")
				.HasComment("Событие активно")
				.IsRequired();

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Events)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ModifierScripts)
				.WithOne(x => x.Event)
				.HasForeignKey(x => x.EventId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(Event.Game));
			gameNavigation.SetField(Event.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
