using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для действия
	/// </summary>
	public class ActionConfiguration : EntityBaseConfiguration<Action>
	{
		/// <summary>
		/// Конфигурация для действия
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Action> builder)
		{
			builder.ToTable("Actions", "InteractionRules")
				.HasComment("Действия");

			builder.Property(x => x.InteractionId)
				.HasColumnName("InteractionId")
				.HasComment("Айди взаимодействия для деятельности")
				.IsRequired();

			builder.Property(x => x.ScenarioActionId)
				.HasColumnName("ScenarioActionId")
				.HasComment("Айди сценария действия во взаимодействии")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название действия")
				.IsRequired();

			builder.Property(x => x.Description)
				.HasColumnName("Description")
				.HasComment("Описание действия");

			builder.HasOne(x => x.Interaction)
				.WithMany(x => x.Actions)
				.HasForeignKey(x => x.InteractionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ScenarioAction)
				.WithOne(x => x.Action)
				.HasForeignKey<Action>(x => x.ScenarioActionId)
				.HasPrincipalKey<Script>(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.ActivityActions)
				.WithOne(x => x.Action)
				.HasForeignKey(x => x.ActionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var interactionNavigation = builder.Metadata.FindNavigation(nameof(Action.Interaction));
			interactionNavigation.SetField(Action.InteractionField);
			interactionNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var scenarioActionNavigation = builder.Metadata.FindNavigation(nameof(Action.ScenarioAction));
			scenarioActionNavigation.SetField(Action.ScenarioActionField);
			scenarioActionNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
