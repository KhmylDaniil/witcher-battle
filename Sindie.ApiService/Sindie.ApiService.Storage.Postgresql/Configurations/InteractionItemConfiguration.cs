using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для предмета во взаимодействии
	/// </summary>
	public class InteractionItemConfiguration : EntityBaseConfiguration<InteractionItem>
	{
		/// <summary>
		/// Конфигурация для предмета во взаимодействии
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<InteractionItem> builder)
		{
			builder.ToTable("InteractionItems", "InteractionRules")
				.HasComment("Предметы во взаимодействии");

			builder.Property(x => x.InteractionsRoleId)
				.HasColumnName("InteractionsRoleId")
				.HasComment("Айди роли взаимодействия")
				.IsRequired();

			builder.Property(x => x.ActivityId)
				.HasColumnName("ActivityId")
				.HasComment("Айди деятельности во взаимодействии")
				.IsRequired();

			builder.Property(x => x.ItemId)
				.HasColumnName("ItemId")
				.HasComment("Айди предмета во взаимодействии")
				.IsRequired();

			builder.Property(x => x.ExpendQuantity)
				.HasColumnName("ExpendQuantity")
				.HasComment("Максимальное количество применений")
				.IsRequired();

			builder.HasOne(x => x.InteractionsRole)
				.WithMany(x => x.InteractionItems)
				.HasForeignKey(x => x.InteractionsRoleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Activity)
				.WithMany(x => x.InteractionItems)
				.HasForeignKey(x => x.ActivityId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Item)
				.WithMany(x => x.InteractionItems)
				.HasForeignKey(x => x.ItemId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var roleNavigation = builder.Metadata.FindNavigation(nameof(InteractionItem.InteractionsRole));
			roleNavigation.SetField(InteractionItem.InteractionsRoleField);
			roleNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var activityNavigation = builder.Metadata.FindNavigation(nameof(InteractionItem.Activity));
			activityNavigation.SetField(InteractionItem.ActivityField);
			activityNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var itemNavigation = builder.Metadata.FindNavigation(nameof(InteractionItem.Item));
			itemNavigation.SetField(InteractionItem.ItemField);
			itemNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}

