using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="SkillConfiguration"/>
	/// </summary>
	public class SkillConfiguration : EntityBaseConfiguration<Skill>
	{
		/// <summary>
		/// Конфигурация для <see cref="SkillConfiguration"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<Skill> builder)
		{
			builder.ToTable("Parameters", "GameRules").
				HasComment("Параметры");

			builder.Property(r => r.GameId)
				.HasColumnName("GameId")
				.HasComment("Айди игры")
				.IsRequired();

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название навыка")
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("Description")
				.HasComment("Описание навыка");

			builder.Property(r => r.StatName)
				.HasColumnName("StatName")
				.HasComment("Название корреспондирующей характеристики");

			builder.OwnsOne(p => p.SkillBounds, pb =>
			{
				pb.Property(p => p.MaxValueSkill)
				.HasColumnName("MaxValueSkills")
				.HasComment("Максимальное значение навыка")
				.IsRequired();
				pb.Property(p => p.MinValueSkill)
				.HasColumnName("MinValueSkills")
				.HasComment("Минимальное значение навыка")
				.IsRequired();
			});

			builder.HasOne(x => x.Game)
				.WithMany(x => x.Skills)
				.HasForeignKey(x => x.GameId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureSkills)
				.WithOne(x => x.Skill)
				.HasForeignKey(x => x.SkillId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.CreatureTemplateSkills)
				.WithOne(x => x.Skill)
				.HasForeignKey(x => x.SkillId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.AbilitiesForAttack)
				.WithOne(x => x.AttackSkill)
				.HasForeignKey(x => x.AttackSkillId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.AbilitiesForDefense)
				.WithMany(x => x.DefensiveSkills)
				.UsingEntity(x => x.ToTable("DefensiveSkills", "GameRules"));

			var gameNavigationGame = builder.Metadata.FindNavigation(nameof(Skill.Game));
			gameNavigationGame.SetField(Skill.GameField);
			gameNavigationGame.SetPropertyAccessMode(PropertyAccessMode.Field);
		}
	}
}
