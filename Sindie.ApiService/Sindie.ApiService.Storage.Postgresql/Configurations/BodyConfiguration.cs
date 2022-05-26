using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности тело
	/// </summary>
	public class BodyConfiguration : EntityBaseConfiguration<Body>
	{
		/// <summary>
		/// Конфигурация для сущности тело
		/// </summary>
		/// <param name="builder">строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Body> builder)
		{
			builder.ToTable("Bodies", "GameInstance")
				.HasComment("Тела");
			
			builder.Property(x => x.CharacterId)
				.HasColumnName("CharacterId")
				.HasComment("Айди персонажа")
				.IsRequired();

			builder.Property(x => x.SlotId)
				.HasColumnName("SlotId")
				.HasComment("Айди слота")
				.IsRequired();

			builder.Property(x => x.ItemId)
				.HasColumnName("ItemId")
				.HasComment("Айди предмета");

			builder.HasOne(x => x.Character)
				.WithMany(x => x.Bodies)
				.HasForeignKey(x => x.CharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);
			
			builder.HasOne(x => x.Slot)
				.WithMany(x => x.Bodies)
				.HasForeignKey(x => x.SlotId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Item)
				.WithMany(x => x.Bodies)
				.HasForeignKey(x => x.ItemId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			var characterNavigation = builder.Metadata.FindNavigation(nameof(Body.Character));
			characterNavigation.SetField(Body.CharacterField);
			characterNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var itemNavigation = builder.Metadata.FindNavigation(nameof(Body.Item));
			itemNavigation.SetField(Body.ItemField);
			itemNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var slotNavigation = builder.Metadata.FindNavigation(nameof(Body.Slot));
			slotNavigation.SetField(Body.SlotField);
			slotNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}