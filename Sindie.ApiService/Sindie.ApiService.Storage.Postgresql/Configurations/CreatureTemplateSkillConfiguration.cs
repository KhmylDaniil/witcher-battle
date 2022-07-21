using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="CreatureTemplateSkill"/>
	/// </summary>
	public class CreatureTemplateSkillConfiguration : EntityBaseConfiguration<CreatureTemplateSkill>
	{
		/// <summary>
		/// Конфигурация для <see cref="CreatureTemplateSkill"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<CreatureTemplateSkill> builder)
		{
			builder.ToTable("CreatureTemplateParameters", "GameRules")
				.HasComment("Параметры шаблона существа");

			builder.Property(r => r.CreatureTemplateId)
				.HasColumnName("CreatureId")
				.HasComment("Айди шаблона существа")
				.IsRequired();

			builder.Property(r => r.SkillId)
				.HasColumnName("SkillId")
				.HasComment("Айди навыка")
				.IsRequired();

			builder.Property(r => r.SkillValue)
				.HasColumnName("SkillValue")
				.HasComment("Значение навыка")
				.IsRequired();

			builder.Property(r => r.StatName)
				.HasColumnName("StatName")
				.HasComment("Название корреспондирующей характеристики");

			builder.HasOne(x => x.CreatureTemplate)
				.WithMany(x => x.CreatureTemplateSkills)
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Skill)
				.WithMany(x => x.CreatureTemplateSkills)
				.HasForeignKey(x => x.SkillId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var creatureTemplateNavigation = builder.Metadata.FindNavigation(nameof(CreatureTemplateSkill.CreatureTemplate));
			creatureTemplateNavigation.SetField(CreatureTemplateSkill.CreatureTemplateField);
			creatureTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var skillNavigation = builder.Metadata.FindNavigation(nameof(CreatureTemplateSkill.Skill));
			skillNavigation.SetField(CreatureTemplateSkill.SkillField);
			skillNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
