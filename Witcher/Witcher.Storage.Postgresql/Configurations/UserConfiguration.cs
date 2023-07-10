using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using System;

namespace Witcher.Storage.Postgresql.Configurations
{
	/// <summary>
	/// Конфигурация для пользователя <see cref="User"/>
	/// </summary>
	public class UserConfiguration : EntityBaseConfiguration<User>
	{
		/// <summary>
		/// Конфигурация модели
		/// </summary>
		/// <param name="builder">Строитель</param>
		public override void ConfigureChild(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users", "System")
				.HasComment("Пользователи");

			builder.Property(r => r.Name)
				.HasColumnName("Name")
				.HasComment("Имя пользователя")
				.IsRequired();

			builder.HasIndex(x => x.Name)
				.IsUnique();

			builder.Property(r => r.Email)
				.HasColumnName("Email")
				.HasComment("Емэйл пользователя");

			builder.Property(r => r.Phone)
				.HasColumnName("Phone")
				.HasComment("Телефон пользователя");

			builder.Property(r => r.InterfaceId)
				.HasColumnName("InterfaceId")
				.HasComment("Интерфейс пользователя")
				.IsRequired();

			builder.Property(r => r.AvatarId)
				.HasColumnName("AvatarId")
				.HasComment("Айди аватара");

			builder.HasMany(x => x.UserAccounts)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.UserRoles)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.UserGames)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasMany(x => x.Notifications)
				.WithOne(x => x.User)
				.HasForeignKey(x => x.UserId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.Cascade);

			builder.HasOne(x => x.Interface)
				.WithMany(x => x.Users)
				.HasForeignKey(x => x.InterfaceId)
				.HasPrincipalKey(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasMany(x => x.ImgFiles)
				.WithMany(x => x.Users);

			builder.HasMany(x => x.TextFiles)
				.WithMany(x => x.Users);

			builder.HasOne(x => x.Avatar)
				.WithOne(x => x.UserAvatar)
				.HasForeignKey<User>(x => x.AvatarId)
				.HasPrincipalKey<ImgFile>(x => x.Id)
				.OnDelete(DeleteBehavior.SetNull);

			builder.HasData(new User
			(
				id: SystemUsers.SystemUserId,
				createdOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				modifiedOn: new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
				createdByUserId: SystemUsers.SystemUserId,
				modifiedByUserId: SystemUsers.SystemUserId,
				name: "Системный пользователь",
				email: "andmin@email.ru",
				phone: "Нет",
				interfaceId: SystemInterfaces.SystemDarkId
				));
		}
	}
}
