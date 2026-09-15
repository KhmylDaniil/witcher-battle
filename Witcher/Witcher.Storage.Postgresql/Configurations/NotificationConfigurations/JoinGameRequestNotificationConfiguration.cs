using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Notifications;

namespace Witcher.Storage.Postgresql.Configurations.NotificationConfigurations
{
	public class JoinGameRequestNotificationConfiguration : HierarchyConfiguration<JoinGameRequestNotification>
	{
		public override void ConfigureChild(EntityTypeBuilder<JoinGameRequestNotification> builder)
		{
			builder.ToTable("JoinGameRequestNotifications", "Notifications")
				.HasComment("Уведомления о запросах присоединения к игре");

			builder.Property(x => x.GameId)
				.HasColumnName("GameId")
				.IsRequired();

			builder.Property(x => x.SenderId)
				.HasColumnName("SenderId")
				.IsRequired();
		}
	}
}
