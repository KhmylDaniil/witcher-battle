using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;


namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация уведомления об удаленных вещах
	/// </summary>
	public class NotificationDeletedItemConfiguration : HierarchyConfiguration<NotificationDeletedItem>
	{
		/// <summary>
		/// Конфигурация уведомления об удаленных вещах
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<NotificationDeletedItem> builder)
		{
			builder.ToTable("NotificationsDeletedItems", "Notifications").
				HasComment("Модификаторы");

			builder.Property(x => x.BagId)
				.HasColumnName("BagId")
				.HasComment("Айди сумки")
				.IsRequired();

			builder.HasOne(x => x.Bag)
				.WithMany(x => x.NotificationDeletedItems)
				.HasForeignKey(x => x.BagId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var BagNavigation = builder.Metadata.FindNavigation(nameof(NotificationDeletedItem.Bag));
			BagNavigation.SetField(NotificationDeletedItem.BagField);
			BagNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
