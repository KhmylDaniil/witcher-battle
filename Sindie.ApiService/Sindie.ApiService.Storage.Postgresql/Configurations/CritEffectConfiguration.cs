using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="CritEffect"/>
	/// </summary>
	public class CritEffectConfiguration : HierarchyConfiguration<CritEffect>
	{
		/// <summary>
		/// Конфигурация для <see cref="CritEffect"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<CritEffect> builder)
		{
			builder.ToTable("CritEffects", "Battles")
				.HasComment("Критические эффекты");

			builder.Property(x => x.CreaturePartId)
				.HasColumnName("CreaturePartId")
				.HasComment("Айди части тела")
				.IsRequired();

			var creaturePartNavigation = builder.Metadata.FindNavigation(nameof(CritEffect.CreaturePart));
			creaturePartNavigation.SetField(CritEffect.CreaturePartField);
			creaturePartNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
