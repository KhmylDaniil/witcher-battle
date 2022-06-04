using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
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

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название шаблона")
				.IsRequired();

			builder.Property(r => r.Type)
				.HasColumnName("Type")
				.HasComment("Тип шаблона существа")
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание шаблона");

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

			builder.HasMany(x => x.CreatureTemplateParameters)
				.WithOne(x => x.CreatureTemplate)
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Abilities)
				.WithOne(x => x.CreatureTemplate)
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Creatures)
				.WithOne(x => x.CreatureTemplate)
				.HasForeignKey(x => x.CreatureTemplateId)
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
