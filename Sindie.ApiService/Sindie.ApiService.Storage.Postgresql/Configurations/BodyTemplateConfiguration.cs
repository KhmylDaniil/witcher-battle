using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{

	/// <summary>
	/// Конфигурация для <see cref="BodyTemplate"/>
	/// </summary>
	public class BodyTemplateConfiguration : EntityBaseConfiguration<BodyTemplate>
	{
		/// <summary>
		/// Конфигурация для <see cref="BodyTemplate"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<BodyTemplate> builder)
		{
			builder.ToTable("BodyTemplates", "GameRules").
				HasComment("Шаблоны тел");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();


			builder.Property(r => r.CreatureTemplateId)
				.HasColumnName("CreatureTemplateId")
				.HasComment("Айди шаблона существа")
				.IsRequired();

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название шаблона тела")
				.IsRequired();

			builder.HasOne(x => x.Game)
				.WithMany(x => x.BodyTemplates)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.CreatureTemplate)
				.WithOne(x => x.BodyTemplate)
				.HasForeignKey<BodyTemplate>(x => x.CreatureTemplateId)
				.HasPrincipalKey<CreatureTemplate>(x => x.BodyTemplateId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureBodies)
				.WithOne(x => x.BodyTemplate)
				.HasForeignKey(x => x.BodyTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.OwnsMany(bt => bt.BodyParts, bp =>
			{
				bp.Property(bp => bp.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

				bp.Property(bp => bp.DamageModifer)
				.HasColumnName("DamageModifer")
				.HasComment("Модификатор урона")
				.IsRequired();

				bp.Property(bp => bp.MinToHit)
				.HasColumnName("MinToHit")
				.HasComment("Минимальное значение попадания")
				.IsRequired();

				bp.Property(bp => bp.MaxToHit)
				.HasColumnName("MaxToHit")
				.HasComment("Максимальное значение попадания")
				.IsRequired();

				bp.Property(bp => bp.StartingArmor)
				.HasColumnName("StartingArmor")
				.HasComment("Начальная броня")
				.IsRequired();

				bp.Property(bp => bp.CurrentArmor)
				.HasColumnName("CurrentArmor")
				.HasComment("Текущая броня")
				.IsRequired();
			});

			var gameNavigation = builder.Metadata.FindNavigation(nameof(BodyTemplate.Game));
			gameNavigation.SetField(BodyTemplate.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var creatureTemplateNavigation = builder.Metadata.FindNavigation(nameof(BodyTemplate.CreatureTemplate));
			creatureTemplateNavigation.SetField(BodyTemplate.CreatureTemplateField);
			creatureTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
