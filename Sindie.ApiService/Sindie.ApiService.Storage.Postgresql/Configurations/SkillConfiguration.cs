using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using System;

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
			builder.ToTable("Skills", "GameRules").
				HasComment("Навыки");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название навыка")
				.IsRequired();

			builder.Property(r => r.StatName)
				.HasColumnName("StatName")
				.HasComment("Название корреспондирующей характеристики");

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

			builder.HasData(new Skill
			(
				name: Skills.BrawlingName,
				id: Skills.BrawlingId,
				statName: "Ref",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.DodgeName,
				id: Skills.DodgeId,
				statName: "Ref",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.MeleeName,
				id: Skills.MeleeId,
				statName: "Ref",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.SmallBladesName,
				id: Skills.SmallBladesId,
				statName: "Ref",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.StaffName,
				id: Skills.StaffId,
				statName: "Ref",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.SwordName,
				id: Skills.SwordId,
				statName: "Ref",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.ArcheryName,
				id: Skills.ArcheryId,
				statName: "Dex",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.AthleticsName,
				id: Skills.AthleticsId,
				statName: "Dex",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.CrossbowName,
				id: Skills.CrossbowId,
				statName: "Dex",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.EnduranceName,
				id: Skills.EnduranceId,
				statName: "Body",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.FirstAidName,
				id: Skills.FirstAidId,
				statName: "Cra",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.SpellName,
				id: Skills.SpellId,
				statName: "Will",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.ResistMagicName,
				id: Skills.ResistMagicId,
				statName: "Will",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.ResistCoercionName,
				id: Skills.ResistCoercionId,
				statName: "Will",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.NeedlingName,
				id: Skills.NeedlingId,
				statName: "Emp",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.EyeGougeName,
				id: Skills.EyeGougeId,
				statName: "Dex",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.BleedingWoundName,
				id: Skills.BleedingWoundId,
				statName: "Int",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.HealingHandsName,
				id: Skills.HealingHandsId,
				statName: "Cra",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Skill
			(
				name: Skills.PhysiqueName,
				id: Skills.PhysiqueId,
				statName: "Body",
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));
		}
	}
}
