using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="BodyPartType"/>
	/// </summary>
	public class BodyPartTypeConfiguration : EntityBaseConfiguration<BodyPartType>
	{
		/// <summary>
		/// Конфигурация для <see cref="BodyPartType"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<BodyPartType> builder)
		{
			builder.ToTable("BodyPartTypes", "GameRules")
				.HasComment("Типы частей тела");

			builder.Property(bpt => bpt.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.HasMany(x => x.BodyParts)
				.WithOne(x => x.BodyPartType)
				.HasForeignKey(x => x.BodyPartTypeId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasData(new BodyPartType
			(
				name: BodyPartTypes.HeadName,
				id: BodyPartTypes.HeadId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new BodyPartType
			(
				name: BodyPartTypes.TorsoName,
				id: BodyPartTypes.TorsoId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new BodyPartType
			(
				name: BodyPartTypes.ArmName,
				id: BodyPartTypes.ArmId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new BodyPartType
			(
				name: BodyPartTypes.LegName,
				id: BodyPartTypes.LegId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new BodyPartType
			(
				name: BodyPartTypes.WingName,
				id: BodyPartTypes.WingId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new BodyPartType
			(
				name: BodyPartTypes.TailName,
				id: BodyPartTypes.TailId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new BodyPartType
			(
				name: BodyPartTypes.VoidName,
				id: BodyPartTypes.VoidId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));
		}
	}
}
