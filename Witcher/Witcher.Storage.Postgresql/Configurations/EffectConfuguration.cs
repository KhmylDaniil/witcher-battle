using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.MySql.Configurations
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

			builder.HasOne(x => x.Creature)
				.WithMany(x => x.Effects)
				.HasForeignKey(x => x.CreatureId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var creatureNavigation = builder.Metadata.FindNavigation(nameof(Effect.Creature));
			creatureNavigation.SetField(Effect.CreatureField);
			creatureNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
