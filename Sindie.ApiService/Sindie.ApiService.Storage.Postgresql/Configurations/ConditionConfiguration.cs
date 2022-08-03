using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using System;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="Condition"/>
	/// </summary>
	public class ConditionConfiguration: EntityBaseConfiguration<Condition>
	{
		/// <summary>
		/// Конфигурация для <see cref="Condition"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<Condition> builder)
		{
			builder.ToTable("Conditions", "GameRules").
				HasComment("Состояния");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название состояния")
				.IsRequired();

			builder.HasMany(x => x.Effects)
				.WithOne(x => x.Condition)
				.HasForeignKey(x => x.EffectId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.AppliedConditions)
				.WithOne(x => x.Condition)
				.HasForeignKey(x => x.ConditionId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasData(new Condition
			(
				name: Conditions.BleedName,
				id: Conditions.BleedId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Conditions.BleedingWoundName,
				id: Conditions.BleedingWoundId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Conditions.PoisonName,
				id: Conditions.PoisonId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Conditions.FireName,
				id: Conditions.FireId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Conditions.FreezeName,
				id: Conditions.FreezeId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Conditions.StunName,
				id: Conditions.StunId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Conditions.StaggeredName,
				id: Conditions.StaggeredId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Conditions.IntoxicationName,
				id: Conditions.IntoxicationId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Conditions.HallutinationName,
				id: Conditions.HallutinationId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Conditions.NauseaName,
				id: Conditions.NauseaId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Conditions.SufflocationName,
				id: Conditions.SufflocationId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Conditions.BlindedName,
				id: Conditions.BlindedId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.SimpleArm,
				id: Crit.SimpleArmId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.SimpleHead1,
				id: Crit.SimpleHead1Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.SimpleHead2,
				id: Crit.SimpleHead2Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.SimpleLeg,
				id: Crit.SimpleLegId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.SimpleTail,
				id: Crit.SimpleTailId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.SimpleTorso1,
				id: Crit.SimpleTorso1Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.SimpleTorso2,
				id: Crit.SimpleTorso2Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.SimpleWing,
				id: Crit.SimpleWingId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.ComplexArm,
				id: Crit.ComplexArmId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.ComplexHead1,
				id: Crit.ComplexHead1Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.ComplexHead2,
				id: Crit.ComplexHead2Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.ComplexLeg,
				id: Crit.ComplexLegId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.ComplexTail,
				id: Crit.ComplexTailId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.ComplexTorso1,
				id: Crit.ComplexTorso1Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.ComplexTorso2,
				id: Crit.ComplexTorso2Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.ComplexWing,
				id: Crit.ComplexWingId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DifficultArm,
				id: Crit.DifficultArmId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DifficultHead1,
				id: Crit.DifficultHead1Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DifficultHead2,
				id: Crit.DifficultHead2Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DifficultLeg,
				id: Crit.DifficultLegId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DifficultTail,
				id: Crit.DifficultTailId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DifficultTorso1,
				id: Crit.DifficultTorso1Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DifficultTorso2,
				id: Crit.DifficultTorso2Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DifficultWing,
				id: Crit.DifficultWingId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DeadlyArm,
				id: Crit.DeadlyArmId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DeadlyHead1,
				id: Crit.DeadlyHead1Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DeadlyHead2,
				id: Crit.DeadlyHead2Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DeadlyLeg,
				id: Crit.DeadlyLegId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DeadlyTail,
				id: Crit.DeadlyTailId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DeadlyTorso1,
				id: Crit.DeadlyTorso1Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DeadlyTorso2,
				id: Crit.DeadlyTorso2Id,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new Condition
			(
				name: Crit.DeadlyWing,
				id: Crit.DeadlyWingId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));
		}
	}
}
