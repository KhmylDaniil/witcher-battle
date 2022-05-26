using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для действия деятельности
	/// </summary>
	public class ActivityActionConfiguration : EntityBaseConfiguration<ActivityAction>
	{
		/// <summary>
		/// Конфигурация для действия деятельности
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<ActivityAction> builder)
		{
			builder.ToTable("ActivityActions", "InteractionRules")
				.HasComment("Действия деятельности");

			builder.Property(x => x.ActivityId)
				.HasColumnName("ActivityId")
				.HasComment("Айди деятельности")
				.IsRequired();

			builder.Property(x => x.ActionId)
				.HasColumnName("ActionId")
				.HasComment("Айди действия")
				.IsRequired();

			builder.Property(x => x.Order)
				.HasColumnName("Order")
				.HasComment("Порядок действий")
				.IsRequired();

			builder.HasOne(x => x.Activity)
				.WithMany(x => x.ActivityActions)
				.HasForeignKey(x => x.ActivityId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Action)
				.WithMany(x => x.ActivityActions)
				.HasForeignKey(x => x.ActionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var activityNavigation = builder.Metadata.FindNavigation(nameof(ActivityAction.Activity));
			activityNavigation.SetField(ActivityAction.ActivityField);
			activityNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var actionNavigation = builder.Metadata.FindNavigation(nameof(ActivityAction.Action));
			actionNavigation.SetField(ActivityAction.ActionField);
			actionNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
