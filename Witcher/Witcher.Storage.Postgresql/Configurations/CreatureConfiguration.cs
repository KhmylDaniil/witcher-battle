using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;
using static Witcher.Core.BaseData.Enums;
using System;

namespace Witcher.Storage.Postgresql.Configurations
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
			builder.ToTable("Creatures", "Battles").
				HasComment("Существа");

			builder.Property(r => r.BattleId)
				.HasColumnName("BattleId")
				.HasComment("Айди боя");

			builder.Property(r => r.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла");

			builder.Property(r => r.CreatureTemplateId)
				.HasColumnName("CreatureTemplateId")
				.HasComment("Айди шаблона существа")
				.IsRequired();

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название существа")
				.IsRequired();

			builder.Property(r => r.CreatureType)
				.HasColumnName("CreatureType")
				.HasComment("Тип существа")
				.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<CreatureType>(v))
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание");

			builder.Property(r => r.HP)
			.HasColumnName("HP")
			.HasComment("Хиты")
			.IsRequired();

			builder.Property(r => r.Sta)
			.HasColumnName("Sta")
			.HasComment("Стамина")
			.IsRequired();

			builder.Property(r => r.Int)
			.HasColumnName("Int")
			.HasComment("Интеллект")
			.IsRequired();

			builder.Property(r => r.Ref)
			.HasColumnName("Ref")
			.HasComment("Рефлексы")
			.IsRequired();

			builder.Property(r => r.Dex)
			.HasColumnName("Dex")
			.HasComment("Ловкость")
			.IsRequired();

			builder.Property(r => r.Body)
			.HasColumnName("Body")
			.HasComment("Телосложение")
			.IsRequired();

			builder.Property(r => r.Emp)
			.HasColumnName("Emp")
			.HasComment("Эмпатия")
			.IsRequired();

			builder.Property(r => r.Cra)
			.HasColumnName("Cra")
			.HasComment("Крафт")
			.IsRequired();

			builder.Property(r => r.Will)
			.HasColumnName("Will")
			.HasComment("Воля")
			.IsRequired();

			builder.Property(r => r.Speed)
			.HasColumnName("Speed")
			.HasComment("Скорость")
			.IsRequired();

			builder.Property(r => r.Luck)
			.HasColumnName("Luck")
			.HasComment("Удача")
			.IsRequired();

			builder.Property(r => r.MaxHP)
			.HasColumnName("MaxHP")
			.HasComment("Максимальные хиты")
			.IsRequired();

			builder.Property(r => r.MaxSta)
			.HasColumnName("MaxSta")
			.HasComment("Максимальная стамина")
			.IsRequired();

			builder.Property(r => r.MaxInt)
			.HasColumnName("MaxInt")
			.HasComment("Максимальный интеллект")
			.IsRequired();

			builder.Property(r => r.MaxRef)
			.HasColumnName("MaxRef")
			.HasComment("Максимальные рефлексы")
			.IsRequired();

			builder.Property(r => r.MaxDex)
			.HasColumnName("MaxDex")
			.HasComment("Максимальна ловкость")
			.IsRequired();

			builder.Property(r => r.MaxBody)
			.HasColumnName("MaxBody")
			.HasComment("Максимальное телосложение")
			.IsRequired();

			builder.Property(r => r.MaxEmp)
			.HasColumnName("MaxEmp")
			.HasComment("Максимальная эмпатия")
			.IsRequired();

			builder.Property(r => r.MaxCra)
			.HasColumnName("MaxCra")
			.HasComment("Максимальный крафт")
			.IsRequired();

			builder.Property(r => r.MaxWill)
			.HasColumnName("MaxWill")
			.HasComment("Максимальная воля")
			.IsRequired();

			builder.Property(r => r.MaxSpeed)
			.HasColumnName("MaxSpeed")
			.HasComment("Максимальная скорость")
			.IsRequired();

			builder.Property(r => r.MaxLuck)
			.HasColumnName("MaxLuck")
			.HasComment("Максимальная удача")
			.IsRequired();

			builder.Property(r => r.Stun)
			.HasColumnName("Stun")
			.HasComment("Устойчивость")
			.IsRequired();

			builder.Property(r => r.LeadingArmId)
			.HasColumnName("LeadingArmId")
			.HasComment("Айди ведущей руки");

			builder.Property(r => r.InitiativeInBattle)
			.HasColumnName("InitiativeInBattle")
			.HasComment("Значение инициативы в битве")
			.IsRequired();

			builder.HasOne(x => x.Battle)
				.WithMany(x => x.Creatures)
				.HasForeignKey(x => x.BattleId)
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

			builder.HasMany(x => x.CreatureSkills)
				.WithOne(x => x.Creature)
				.HasForeignKey(x => x.CreatureId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Abilities)
				.WithMany(x => x.Creatures)
				.UsingEntity(x => x.ToTable("CreatureAbilities", "Battles"));

			builder.HasMany(x => x.Effects)
				.WithOne(x => x.Creature)
				.HasForeignKey(x => x.CreatureId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.OwnsMany(x => x.DamageTypeModifiers)
				.HasKey(r => r.Id);

			builder.HasMany(x => x.CreatureParts)
				.WithOne(x => x.Creature)
				.HasForeignKey(x => x.CreatureId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var battleNavigation = builder.Metadata.FindNavigation(nameof(Creature.Battle));
			battleNavigation.SetField(Creature.BattleField);
			battleNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(Creature.ImgFile));
			imgFileNavigation.SetField(Creature.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var creatureTemplateNavigation = builder.Metadata.FindNavigation(nameof(Creature.CreatureTemplate));
			creatureTemplateNavigation.SetField(Creature.CreatureTemplateField);
			creatureTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
