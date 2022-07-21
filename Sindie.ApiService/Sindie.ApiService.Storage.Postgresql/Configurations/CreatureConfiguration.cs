using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

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
			builder.ToTable("Creatures", "Battles").
				HasComment("Существа");

			builder.Property(r => r.BattleId)
				.HasColumnName("BattleId")
				.HasComment("Айди боя")
				.IsRequired();

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

			builder.Property(r => r.CreatureTypeId)
				.HasColumnName("CreatureTypeId")
				.HasComment("Айди типа существа")
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

			builder.HasOne(x => x.CreatureType)
				.WithMany(x => x.Creatures)
				.HasForeignKey(x => x.CreatureTypeId)
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

			builder.HasMany(x => x.Conditions)
				.WithMany(x => x.Creatures)
				.UsingEntity(x => x.ToTable("CurrentConditions", "Battles"));

			builder.HasMany(x => x.Vulnerables)
				.WithMany(x => x.VulnerableCreatures)
				.UsingEntity(x => x.ToTable("CreatureVulnerables", "Battles"));

			builder.HasMany(x => x.Resistances)
				.WithMany(x => x.ResistantCreatures)
				.UsingEntity(x => x.ToTable("CreatureResistances", "Battles"));

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

			var creatureTypeNavigation = builder.Metadata.FindNavigation(nameof(Creature.CreatureType));
			creatureTypeNavigation.SetField(Creature.CreatureTypeField);
			creatureTypeNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
