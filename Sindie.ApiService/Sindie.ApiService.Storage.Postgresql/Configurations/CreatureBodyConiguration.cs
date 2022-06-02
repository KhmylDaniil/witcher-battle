using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="CreatureBody"/>
	/// </summary>
	public class CreatureBodyConiguration : EntityBaseConfiguration<CreatureBody>
	{
		/// <summary>
		/// Конфигурация для <see cref="CreatureBody"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<CreatureBody> builder)
		{
			builder.ToTable("CreatureBodies", "GameInstance").
				HasComment("Шаблоны тел");

			builder.Property(r => r.InstanceId)
				.HasColumnName("InstanceId")
				.HasComment("Айди экземпляра")
				.IsRequired();

			builder.Property(r => r.CreatureId)
				.HasColumnName("CreatureId")
				.HasComment("Айди сущесттва")
				.IsRequired();

			builder.Property(r => r.BodyTemplateId)
				.HasColumnName("BodyTemplateId")
				.HasComment("Айди шаблона тела")
				.IsRequired();

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название тела существа")
				.IsRequired();

			builder.HasOne(x => x.Instance)
				.WithMany(x => x.CreatureBodies)
				.HasForeignKey(x => x.InstanceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Creature)
				.WithOne(x => x.CreatureBody)
				.HasForeignKey<CreatureBody>(x => x.CreatureId)
				.HasPrincipalKey<Creature>(x => x.CreatureBodyId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.BodyTemplate)
				.WithMany(x => x.CreatureBodies)
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

			var instanceNavigation = builder.Metadata.FindNavigation(nameof(CreatureBody.Instance));
			instanceNavigation.SetField(CreatureBody.InstanceField);
			instanceNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var creatureNavigation = builder.Metadata.FindNavigation(nameof(CreatureBody.Creature));
			creatureNavigation.SetField(CreatureBody.CreatureField);
			creatureNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var bodyTemplateNavigation = builder.Metadata.FindNavigation(nameof(CreatureBody.BodyTemplate));
			bodyTemplateNavigation.SetField(CreatureBody.BodyTemplateField);
			bodyTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
