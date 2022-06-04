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
	/// <summary>
	/// Конфигурация для <see cref="Creature"/>
	/// </summary>
	public class CreatureConfiguration : EntityBaseConfiguration<Creature>
	{
		/// <summary>
		/// Конфигурация для <see cref="Creature"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<Creature> builder)
		{
			builder.ToTable("Creatures", "GameInstance").
				HasComment("Существа");

			builder.Property(r => r.InstanceId)
				.HasColumnName("InstanceId")
				.HasComment("Айди экземпляра")
				.IsRequired();

			builder.Property(r => r.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла");

			builder.Property(r => r.CreatureTemplateId)
				.HasColumnName("CreatureTemplateId")
				.HasComment("Айди шаблона существа")
				.IsRequired();

			builder.Property(r => r.BodyTemplateId)
				.HasColumnName("BodyTemplateId")
				.HasComment("Айди шаблона тела")
				.IsRequired();

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название существа")
				.IsRequired();

			builder.Property(r => r.Type)
				.HasColumnName("Type")
				.HasComment("Тип шаблона существа")
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание шаблона");

			builder.HasOne(x => x.Instance)
				.WithMany(x => x.Creatures)
				.HasForeignKey(x => x.InstanceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.Creature)
				.HasForeignKey<Creature>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.CreatureTemplate)
				.WithMany(x => x.Creatures)
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureParameters)
				.WithOne(x => x.Creature)
				.HasForeignKey(x => x.CreatureId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Abilities)
				.WithMany(x => x.Creatures)
				.UsingEntity(x => x.ToTable("CreatureAbilities", "GameInstance"));

			builder.HasMany(x => x.Conditions)
				.WithMany(x => x.Creatures)
				.UsingEntity(x => x.ToTable("CurrentConditions", "GameInstance"));

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

			var instanceNavigation = builder.Metadata.FindNavigation(nameof(Creature.Instance));
			instanceNavigation.SetField(Creature.InstanceField);
			instanceNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(Creature.ImgFile));
			imgFileNavigation.SetField(Creature.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var creatureTemplateNavigation = builder.Metadata.FindNavigation(nameof(Creature.CreatureTemplate));
			creatureTemplateNavigation.SetField(Creature.CreatureTemplateField);
			creatureTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var bodyTemplateNavigation = builder.Metadata.FindNavigation(nameof(Creature.BodyTemplate));
			bodyTemplateNavigation.SetField(Creature.BodyTemplateField);
			bodyTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
