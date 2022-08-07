using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	public class EffectConfuguration : EntityBaseConfiguration<Effect>
	{
		/// <summary>
		/// Конфигурация для <see cref="Effect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<Effect> builder)
		{
			builder.ToTable("Effects", "Battles")
				.HasComment("Эффекты");
			
			builder.Property(x => x.CreatureId)
				.HasColumnName("CreatureId")
				.HasComment("Айди существа")
				.IsRequired();

			builder.Property(x => x.Name)
				.HasColumnName("Name")
				.HasComment("Название эффекта")
				.IsRequired();

			builder.Property(x => x.EffectId)
				.HasColumnName("EffectId")
				.HasComment("Айди эффекта")
				.IsRequired();

			builder.Property(x => x.CreaturePartId)
				.HasColumnName("CreaturePartId")
				.HasComment("Айди части тела")
				.IsRequired();

			builder.HasOne(x => x.Creature)
				.WithMany(x => x.Effects)
				.HasForeignKey(x => x.CreatureId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var creatureNavigation = builder.Metadata.FindNavigation(nameof(Effect.Creature));
			creatureNavigation.SetField(Effect.CreatureField);
			creatureNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var conditionNavigation = builder.Metadata.FindNavigation(nameof(Effect.Condition));
			conditionNavigation.SetField(Effect.ConditionField);
			conditionNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var creaturePartNavigation = builder.Metadata.FindNavigation(nameof(Effect.CreaturePart));
			creaturePartNavigation.SetField(Effect.CreaturePartField);
			creaturePartNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
