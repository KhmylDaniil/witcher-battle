using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		}
	}
}
