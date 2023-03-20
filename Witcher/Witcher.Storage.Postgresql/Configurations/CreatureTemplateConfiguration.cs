using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;
using static Witcher.Core.BaseData.Enums;
using System;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="CreatureTemplate"/>
	/// </summary>
	public class CreatureTemplateConfiguration : EntityBaseConfiguration<CreatureTemplate>
	{

		/// <summary>
		/// Конфигурация для <see cref="CreatureTemplate"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<CreatureTemplate> builder)
		{
			builder.ToTable("CreatureTemplates", "GameRules").
				HasComment("Шаблоны существ");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(r => r.ImgFileId)
				.HasColumnName("ImgFileId")
				.HasComment("Айди графического файла");

			builder.Property(r => r.BodyTemplateId)
				.HasColumnName("BodyTemplateId")
				.HasComment("Айди шаблона тела")
				.IsRequired();

			builder.Property(r => r.CreatureType)
				.HasColumnName("CreatureType")
				.HasComment("Тип существа")
				.HasConversion(
					v => v.ToString(),
					v => Enum.Parse<CreatureType>(v))
				.IsRequired();

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название шаблона")
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание шаблона");

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

			builder.HasOne(x => x.Game)
				.WithMany(x => x.CreatureTemplates)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.ImgFile)
				.WithOne(x => x.CreatureTemplate)
				.HasForeignKey<CreatureTemplate>(x => x.ImgFileId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasOne(x => x.BodyTemplate)
				.WithMany(x => x.CreatureTemplates)
				.HasForeignKey(x => x.BodyTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureTemplateSkills)
				.WithOne(x => x.CreatureTemplate)
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Abilities)
				.WithMany(x => x.CreatureTemplates)
				.UsingEntity(x => x.ToTable("CreatureTemplateAbilities", "GameRules"));

			builder.HasMany(x => x.Creatures)
				.WithOne(x => x.CreatureTemplate)
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureTemplateParts)
				.WithOne(x => x.CreatureTemplate)
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.OwnsMany(x => x.DamageTypeModifiers)
				.HasKey(r => r.Id);

			var gameNavigation = builder.Metadata.FindNavigation(nameof(CreatureTemplate.Game));
			gameNavigation.SetField(CreatureTemplate.GameField);
			gameNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var imgFileNavigation = builder.Metadata.FindNavigation(nameof(CreatureTemplate.ImgFile));
			imgFileNavigation.SetField(CreatureTemplate.ImgFileField);
			imgFileNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var bodyTemplateNavigation = builder.Metadata.FindNavigation(nameof(CreatureTemplate.BodyTemplate));
			bodyTemplateNavigation.SetField(CreatureTemplate.BodyTemplateField);
			bodyTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
