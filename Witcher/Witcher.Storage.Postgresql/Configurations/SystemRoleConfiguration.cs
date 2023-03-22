using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using System;

namespace Witcher.Storage.MySql.Configurations
{
	/// <summary>
	/// Конфигурация для роли в системе <see cref="SystemRole"/>
	/// </summary>
	public class SystemRoleConfiguration : EntityBaseConfiguration<SystemRole>
	{
		/// <summary>
		/// Конфигурация для роли в системе
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<SystemRole> builder)
		{
			builder.ToTable("SystemRoles", "System")
				.HasComment("Роли в системе");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Роль в системе")
				.IsRequired();

			builder.HasIndex(x => x.Name)
				.IsUnique();

			builder.HasMany(x => x.UserRoles)
				.WithOne(x => x.SystemRole)
				.HasForeignKey(x => x.SystemRoleId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasData(new SystemRole
			(
				name: SystemRoles.AdminRoleName,
				id: SystemRoles.AdminRoleId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new SystemRole
			(
				name: SystemRoles.UserRoleName,
				id: SystemRoles.UserRoleId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));
		}
	}
}
