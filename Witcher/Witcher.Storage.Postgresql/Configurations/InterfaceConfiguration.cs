using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using System;

namespace Witcher.Storage.MySql.Configurations
{
	/// <summary>
	/// Конфигурация для сущности интерфейс
	/// </summary>
	public class InterfaceConfiguration : EntityBaseConfiguration<Interface>
	{
		/// <summary>
		/// Конфигурация для сущности интерфейс
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<Interface> builder)
		{
			builder.ToTable("Interfaces", "System")
				.HasComment("Интерфейсы");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Название интерфейса")
				.IsRequired();

			builder.HasIndex(x => x.Name)
				.IsUnique();

			builder.Property(r => r.Type)
				.HasColumnName("Type")
				.HasComment("Тип интерфейса")
				.IsRequired();

			builder.HasMany(x => x.Users)
				.WithOne(x => x.Interface)
				.HasForeignKey(x => x.InterfaceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.UserGames)
				.WithOne(x => x.Interface)
				.HasForeignKey(x => x.InterfaceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.UserGameCharacters)
				.WithOne(x => x.Interface)
				.HasForeignKey(x => x.InterfaceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasData(new Interface
			(
				name: SystemInterfaces.SystemDarkName,
				type: InterfaceType.System,
				id: SystemInterfaces.SystemDarkId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId
				));

			builder.HasData(new Interface
			(
				name: SystemInterfaces.SystemLightName,
				type: InterfaceType.System,
				id: SystemInterfaces.SystemLightId,
				createdOn: new System.DateTime(2020, 1, 1),
				modifiedOn: new System.DateTime(2020, 1, 1),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId
				));

			builder.HasData(new Interface
			(
				name: SystemInterfaces.GameDarkName,
				type: InterfaceType.Game,
				id: SystemInterfaces.GameDarkId,
				createdOn: new System.DateTime(2020, 1, 1),
				modifiedOn: new System.DateTime(2020, 1, 1),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId
				));

			builder.HasData(new Interface
			(
				name: SystemInterfaces.GameLightName,
				type: InterfaceType.Game,
				id: SystemInterfaces.GameLightId,
				createdOn: new System.DateTime(2020, 1, 1),
				modifiedOn: new System.DateTime(2020, 1, 1),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId
				));

			builder.HasData(new Interface
			(
				name: SystemInterfaces.CharacterDarkName,
				type: InterfaceType.Character,
				id: SystemInterfaces.CharacterDarkId,
				createdOn: new System.DateTime(2020, 1, 1),
				modifiedOn: new System.DateTime(2020, 1, 1),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId
				));

			builder.HasData(new Interface
			(
				name: SystemInterfaces.CharacterLightName,
				type: InterfaceType.Character,
				id: SystemInterfaces.CharacterLightId,
				createdOn: new System.DateTime(2020, 1, 1),
				modifiedOn: new System.DateTime(2020, 1, 1),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId
				));
		}
	}
}