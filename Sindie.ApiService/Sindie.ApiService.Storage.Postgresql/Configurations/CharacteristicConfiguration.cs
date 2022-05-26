using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для характеристики
	/// </summary>
	public class CharacteristicConfiguration : HierarchyConfiguration<Characteristic>
	{
		/// <summary>
		/// Конфигурация для характеристики
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Characteristic> builder)
		{
			builder.ToTable("Characteristics", "InteractionRules")
				.HasComment("Характеристики");

			builder.Property(x => x.InteractionId)
				.HasColumnName("InteractionId")
				.HasComment("Айди взаимодействия")
				.IsRequired();

			builder.Property(x => x.MinCondition)
				.HasColumnName("MinCondition")
				.HasComment("Минимальное значение")
				.IsRequired();

			builder.Property(x => x.MaxCondition)
				.HasColumnName("MaxCondition")
				.HasComment("Максимальное значение")
				.IsRequired();

			builder.HasOne(x => x.Interaction)
				.WithMany(x => x.Characteristics)
				.HasForeignKey(x => x.InteractionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var interactionNavigation = builder.Metadata.FindNavigation(nameof(Characteristic.Interaction));
			interactionNavigation.SetField(Characteristic.InteractionField);
			interactionNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
