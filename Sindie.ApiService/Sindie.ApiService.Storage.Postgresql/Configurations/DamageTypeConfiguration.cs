using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using System;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для <see cref="DamageType"/>
	/// </summary>
	public class DamageTypeConfiguration : EntityBaseConfiguration<DamageType>
	{
		/// <summary>
		/// Конфигурация для <see cref="DamageType"/>
		/// </summary>
		public override void ConfigureChild(EntityTypeBuilder<DamageType> builder)
		{
			builder.ToTable("DamageTypes", "GameRules")
				.HasComment("Типы урона");

			builder.Property(dt => dt.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.HasMany(x => x.Abilities)
				.WithOne(x => x.DamageType)
				.HasForeignKey(x => x.DamageTypeId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.VulnerableCreatures)
				.WithMany(x => x.Vulnerables)
				.UsingEntity(x => x.ToTable("CreatureVulnerables", "Battles"));

			builder.HasMany(x => x.ResistantCreatures)
				.WithMany(x => x.Resistances)
				.UsingEntity(x => x.ToTable("CreatureResistances", "Battles"));

			builder.HasMany(x => x.VulnerableCreatureTemplates)
				.WithMany(x => x.Vulnerables)
				.UsingEntity(x => x.ToTable("CreatureTemplateVulnerables", "GameRules"));

			builder.HasMany(x => x.ResistantCreatureTemplates)
				.WithMany(x => x.Resistances)
				.UsingEntity(x => x.ToTable("CreatureTemplateResistances", "GameRules"));


			builder.HasData(new DamageType
			(
				name: DamageTypes.SlashingName,
				id: DamageTypes.SlashingId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new DamageType
			(
				name: DamageTypes.PiercingName,
				id: DamageTypes.PiercingId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new DamageType
			(
				name: DamageTypes.BludgeoningName,
				id: DamageTypes.BludgeoningId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new DamageType
			(
				name: DamageTypes.ElementalName,
				id: DamageTypes.ElementalId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new DamageType
			(
				name: DamageTypes.FireName,
				id: DamageTypes.FireId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new DamageType
			(
				name: DamageTypes.SilverName,
				id: DamageTypes.SilverId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));
		}
	}
}
