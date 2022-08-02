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
		}
	}
}
