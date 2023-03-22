using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;
using System;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Storage.MySql.Configurations
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
			builder.ToTable("CreatureTemplateSkills", "GameRules")
				.HasComment("Навыки шаблона существа");

			builder.Property(r => r.CreatureTemplateId)
				.HasColumnName("CreatureId")
				.HasComment("Айди шаблона существа")
				.IsRequired();

			builder.Property(r => r.Skill)
				.HasColumnName("Skill")
				.HasComment("Навык")
				.HasConversion(
					v => v.ToString(),
					v => (Skill)Enum.Parse(typeof(Skill), v))
				.IsRequired();

			builder.Property(r => r.SkillValue)
				.HasColumnName("SkillValue")
				.HasComment("Значение навыка")
				.IsRequired();

			builder.HasOne(x => x.CreatureTemplate)
				.WithMany(x => x.CreatureTemplateSkills)
				.HasForeignKey(x => x.CreatureTemplateId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var creatureTemplateNavigation = builder.Metadata.FindNavigation(nameof(CreatureTemplateSkill.CreatureTemplate));
			creatureTemplateNavigation.SetField(CreatureTemplateSkill.CreatureTemplateField);
			creatureTemplateNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
