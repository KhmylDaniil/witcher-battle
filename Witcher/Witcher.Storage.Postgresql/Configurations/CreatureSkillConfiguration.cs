using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.Entities;

namespace Witcher.Storage.Postgresql.Configurations
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
			builder.ToTable("CreatureSkills", "Battles")
				.HasComment("Навыки существа");

			builder.Property(r => r.CreatureId)
				.HasColumnName("CreatureId")
				.HasComment("Айди существа")
				.IsRequired();

			builder.Property(r => r.Skill).HasColumnName("Skill").HasComment("Навык")
				.IsRequired();

			builder.Property(r => r.SkillValue)
				.HasColumnName("SkillValue")
				.HasComment("Значение навыка")
				.IsRequired();

			builder.Property(r => r.MaxValue)
				.HasColumnName("MaxValue")
				.HasComment("Макксимальное значение навыка")
				.IsRequired();

			builder.HasOne(x => x.Creature)
				.WithMany(x => x.CreatureSkills)
				.HasForeignKey(x => x.CreatureId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			var creatureNavigation = builder.Metadata.FindNavigation(nameof(CreatureSkill.Creature));
			creatureNavigation.SetField(CreatureSkill.CreatureField);
			creatureNavigation.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
