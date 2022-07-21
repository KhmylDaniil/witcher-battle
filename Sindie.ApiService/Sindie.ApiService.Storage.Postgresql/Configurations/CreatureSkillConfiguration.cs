using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;
using System;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="CreatureSkill"/>
	/// </summary>
	public class CreatureSkillConfiguration : EntityBaseConfiguration<CreatureSkill>
	{
		/// <summary>
		/// Конфигурация для <see cref="CreatureSkill"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<CreatureSkill> builder)
		{
			builder.ToTable("CreatureParameters", "Battles")
				.HasComment("Параметры существа");

			builder.Property(r => r.CreatureId)
				.HasColumnName("CreatureId")
				.HasComment("Айди существа")
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

			builder.HasOne(x => x.Creature)
				.WithMany(x => x.CreatureSkills)
				.HasForeignKey(x => x.CreatureId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Skill)
				.WithMany(x => x.CreatureSkills)
				.HasForeignKey(x => x.SkillId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var creatureNavigation = builder.Metadata.FindNavigation(nameof(CreatureSkill.Creature));
			creatureNavigation.SetField(CreatureSkill.CreatureField);
			creatureNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);

			var skillNavigation = builder.Metadata.FindNavigation(nameof(CreatureSkill.Skill));
			skillNavigation.SetField(CreatureSkill.SkillField);
			skillNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
