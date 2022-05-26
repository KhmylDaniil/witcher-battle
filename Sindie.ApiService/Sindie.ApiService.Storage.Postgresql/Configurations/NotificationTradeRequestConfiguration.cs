using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация уведомления о предложении передать предметы
	/// </summary>
	public class NotificationTradeRequestConfiguration : HierarchyConfiguration<NotificationTradeRequest>
    {
		/// <summary>
		/// Конфигурация уведомления о предложении передать предметы
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<NotificationTradeRequest> builder)
        {
			builder.ToTable("NotificationTradeRequests", "Notifications").
				 HasComment("Уведомления о намерении передать предметы");

			builder.Property(x => x.SourceBagId)
				.HasColumnName("SourceBagId")
				.HasComment("Айди сумки-отправителя")
				.IsRequired();

			builder.Property(x => x.ReceiveBagId)
				.HasColumnName("ReceiveBagId")
				.HasComment("Айди сумки-получателя")
				.IsRequired();

			builder.OwnsMany(n => n.BagItems, ni =>
			{
				ni.Property(ni => ni.ItemId)
				.HasColumnName("ItemId")
				.HasComment("Айди предмета");
				ni.Property(ni => ni.ItemName)
				.HasColumnName("ItemName")
				.HasComment("Название предмета");
				ni.Property(ni => ni.Quantity)
				.HasColumnName("QuantityItem")
				.HasComment("Количество предметов");
				ni.Property(ni => ni.MaxQuantity)
				.HasColumnName("MaxQuantityItem")
				.HasComment("Максимальное количество предметов");
				ni.Property(ni => ni.Stack)
				.HasColumnName("Stack")
				.HasComment("Стак");
				ni.Property(ni => ni.TotalWeight)
				.HasColumnName("TotalWeight")
				.HasComment("Общий вес стака");
			});

			builder.HasOne(x => x.SourceBag)
				.WithMany(x => x.NotificationsTradeRequestSource)
				.HasForeignKey(x => x.SourceBagId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ReceiveCharacter)
				.WithMany(x => x.NotificationTradeRequests)
				.HasForeignKey(x => x.ReceiveCharacterId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			var sourceBagNavigation = builder.Metadata.FindNavigation(nameof(NotificationTradeRequest.SourceBag));
			sourceBagNavigation.SetField(NotificationTradeRequest.SourceBagField);
			sourceBagNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var receiveCharacterNavigation = builder.Metadata.FindNavigation(nameof(NotificationTradeRequest.ReceiveCharacter));
			receiveCharacterNavigation.SetField(NotificationTradeRequest.ReceiveCharacterField);
			receiveCharacterNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
    }
}
