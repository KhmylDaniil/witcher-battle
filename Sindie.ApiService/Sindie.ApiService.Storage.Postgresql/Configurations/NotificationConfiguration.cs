using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация уведомления
	/// </summary>
	public class NotificationConfiguration : EntityBaseConfiguration<Notification>
	{
		/// <summary>
		/// Конфигурация уведомления
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Notification> builder)
		{
			builder.ToTable("Notifications", "Notifications").
				HasComment("Уведомления");

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.Property(x => x.Message)
				.HasColumnName("Message")
				.HasComment("Сообщение")
				.IsRequired();

			builder.Property(x => x.Duration)
				.HasColumnName("Duration")
				.HasComment("Длительность существования в минутах")
				.IsRequired();

			builder.HasMany(x => x.Receivers)
				.WithMany(x => x.Notifications).
				UsingEntity(j => j.ToTable("UserNotications", "Notifications"));
		}
	}
}
