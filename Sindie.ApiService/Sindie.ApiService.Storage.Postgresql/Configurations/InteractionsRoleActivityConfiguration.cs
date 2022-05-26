using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для деятельности роли взаимодействия
	/// </summary>
	public class InteractionsRoleActivityConfiguration : EntityBaseConfiguration<InteractionsRoleActivity>
	{
		/// <summary>
		/// Конфигурация для деятельности роли взаимодействия
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<InteractionsRoleActivity> builder)
		{
			builder.ToTable("InteractionsRoleActivities", "InteractionRules")
				.HasComment("Деятельности роли взаимодействия");

			builder.Property(x => x.InteractionsRoleId)
				.HasColumnName("InteractionsRoleId")
				.HasComment("Айди роли взаимодействия")
				.IsRequired();

			builder.Property(x => x.ActivityId)
				.HasColumnName("ActivityId")
				.HasComment("Айди деятельности")
				.IsRequired();

			builder.Property(x => x.Order)
				.HasColumnName("Order")
				.HasComment("Порядок деятельности");

			builder.HasOne(x => x.InteractionsRole)
				.WithMany(x => x.InteractionsRoleActivities)
				.HasForeignKey(x => x.InteractionsRoleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Activity)
				.WithMany(x => x.InteractionsRoleActivities)
				.HasForeignKey(x => x.ActivityId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var roleNavigation = builder.Metadata.FindNavigation(nameof(InteractionsRoleActivity.InteractionsRole));
			roleNavigation.SetField(InteractionsRoleActivity.InteractionsRoleField);
			roleNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var activityNavigation = builder.Metadata.FindNavigation(nameof(InteractionsRoleActivity.Activity));
			activityNavigation.SetField(InteractionsRoleActivity.ActivityField);
			activityNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}

