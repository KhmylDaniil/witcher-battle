using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для деятельности
	/// </summary>
	public class ActivityConfiguration : EntityBaseConfiguration<Activity>
	{
		/// <summary>
		/// Конфигурация для деятельности
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Activity> builder)
		{
			builder.ToTable("Activities", "InteractionRules")
				.HasComment("Деятельности");

			builder.Property(x => x.InteractionId)
				.HasColumnName("InteractionId")
				.HasComment("Айди взаимодействия для деятельности")
				.IsRequired();

			builder.Property(x => x.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла для деятельности");

			builder.Property(x => x.ApplicationAreaId)
				.HasColumnName("ApplicationAreaId")
				.HasComment("Айди области применения для деятельности")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название деятельности")
				.IsRequired();

			builder.Property(x => x.Description)
				.HasColumnName("Description")
				.HasComment("Описание деятельности");

			builder.HasOne(x => x.Interaction)
				.WithMany(x => x.Activities)
				.HasForeignKey(x => x.InteractionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.Activity)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.HasForeignKey<Activity>(x => x.ImgFileId)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.ApplicationArea)
				.WithMany(x => x.Activities)
				.HasForeignKey(x => x.ApplicationAreaId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.InteractionsRoleActivities)
				.WithOne(x => x.Activity)
				.HasForeignKey(x => x.ActivityId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.InteractionItems)
				.WithOne(x => x.Activity)
				.HasForeignKey(x => x.ActivityId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ActivityActions)
				.WithOne(x => x.Activity)
				.HasForeignKey(x => x.ActivityId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var interactionNavigation = builder.Metadata.FindNavigation(nameof(Activity.Interaction));
			interactionNavigation.SetField(Activity.InteractionField);
			interactionNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(Activity.ImgFile));
			imgFileNavigation.SetField(Activity.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var applicationAreaNavigation = builder.Metadata.FindNavigation(nameof(Activity.ApplicationArea));
			applicationAreaNavigation.SetField(Activity.ApplicationAreaField);
			applicationAreaNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

		}
	}
}
