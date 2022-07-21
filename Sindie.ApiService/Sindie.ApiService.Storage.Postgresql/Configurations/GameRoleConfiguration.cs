using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using System;

namespace Sindie.ApiService.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности роль в игре справочник
	/// </summary>
	public class GameRoleConfiguration : EntityBaseConfiguration<GameRole>
	{
		/// <summary>
		/// Конфигурация для сущности роль в игре справочник
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<GameRole> builder)
		{
			builder.ToTable("GameRoles", "System")
				.HasComment("Роли в игре");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название")
				.IsRequired();

			builder.HasIndex(x => x.Name)
				.IsUnique();

			builder.HasMany(x => x.UserGames)
			.WithOne(x => x.GameRole)
			.HasForeignKey(x => x.GameRoleId)
			.HasPrincipalKey(x => x.Id)
			.OnDelete(DeleteBehavior.Cascade);

			builder.HasData(new GameRole
			(
				name: GameRoles.MainMasterRoleName,
				id: GameRoles.MainMasterRoleId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new GameRole
			(
				name: GameRoles.MasterRoleName,
				id: GameRoles.MasterRoleId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));

			builder.HasData(new GameRole
			(
				name: GameRoles.PlayerRoleName,
				id: GameRoles.PlayerRoleId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId));
		}
	}
}

