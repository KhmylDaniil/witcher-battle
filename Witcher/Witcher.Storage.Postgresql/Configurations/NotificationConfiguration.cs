using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Notifications;

namespace Witcher.Storage.Postgresql.Configurations
{
	public class NotificationConfiguration : EntityBaseConfiguration<Notification>
	{
		public override void ConfigureChild(EntityTypeBuilder<Notification> builder)
		{
			builder.ToTable("Notifications", "Notifications");

			builder.Property(r => r.UserId)
				.HasColumnName("UserId")
				.IsRequired();

			builder.Property(r => r.Message)
				.HasColumnName("Message");

			builder.HasOne(x => x.User)
				.WithMany(x => x.Notifications)
				.HasForeignKey(x => x.UserId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);


			var userNavigation = builder.Metadata.FindNavigation(nameof(Notification.User));
			userNavigation.SetField(Notification.UserField);
			userNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
