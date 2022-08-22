﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sindie.ApiService.Storage.Postgresql;

namespace Sindie.ApiService.Storage.Postgresql.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210206115025_GameTemplate")]
    partial class GameTemplate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.GameTemplate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("GameTemplates");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"),
                            CreatedByUserId = new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"),
                            CreatedOn = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "andmin@email.ru",
                            ModifiedByUserId = new Guid("8094e0d0-3137-4791-9053-9667cbe107d6"),
                            ModifiedOn = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Системный пользователь",
                            Phone = "Нет"
                        });
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.UserAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.UserAccount", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.User", "User")
                        .WithMany("UserAccounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.UserRole", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sindie.ApiService.Core.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.User", b =>
                {
                    b.Navigation("UserAccounts");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}