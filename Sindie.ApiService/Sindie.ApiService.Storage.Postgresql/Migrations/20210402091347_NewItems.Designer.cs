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
    [Migration("20210402091347_NewItems")]
    partial class NewItems
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Bag", b =>
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

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid");

                    b.Property<int?>("MaxBagSize")
                        .HasColumnType("integer");

                    b.Property<int?>("MaxOccupiedBagSize")
                        .HasColumnType("integer");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Bags");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.BagItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<Guid>("BagId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("OuantityItem")
                        .HasColumnType("integer");

                    b.Property<int>("Stack")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BagId");

                    b.HasIndex("ItemId");

                    b.ToTable("BagItems");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Body", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<Guid>("CharacterId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<int?>("MaxQuantityInSlot")
                        .HasColumnType("integer");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("SlotId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("SlotId");

                    b.ToTable("Body");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.BodyItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<Guid>("CharacterId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("OuantityItem")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("ItemId");

                    b.ToTable("BodyItem");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Character", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<Guid?>("BagId")
                        .HasColumnType("uuid");

                    b.Property<bool>("CharacterInInteraction")
                        .HasColumnType("boolean");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("TimeActivate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("TypeCharacter")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("UserActivateId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BagId")
                        .IsUnique();

                    b.HasIndex("GameId");

                    b.HasIndex("UserActivateId")
                        .IsUnique();

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.CharacterParameter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<Guid>("CharacterId")
                        .HasColumnType("uuid");

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

                    b.Property<Guid>("ParameterId")
                        .HasColumnType("uuid");

                    b.Property<double>("ParameterValue")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("ParameterId");

                    b.ToTable("CharacterParameters");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Game", b =>
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

                    b.Property<DateTime?>("DateOfGame")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("StoryAboutRules")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Games");
                });

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

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Item", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<bool?>("AutoWear")
                        .HasColumnType("boolean");

                    b.Property<Guid>("CreatedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("now() at time zone 'utc'");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool?>("IsRemovable")
                        .HasColumnType("boolean");

                    b.Property<int?>("MaxQuantityItem")
                        .HasColumnType("integer");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double?>("OccupiedBagSize")
                        .HasColumnType("double precision");

                    b.Property<Guid>("SlotId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SlotId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Parameter", b =>
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

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid");

                    b.Property<double?>("MaxValueParameter")
                        .HasColumnType("double precision");

                    b.Property<double?>("MinValueParameter")
                        .HasColumnType("double precision");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Parameters");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.ParameterItem", b =>
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

                    b.Property<Guid>("ItemId")
                        .HasColumnType("uuid");

                    b.Property<double>("ItemValue")
                        .HasColumnType("double precision");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("ParameterId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ParameterId");

                    b.ToTable("ParameterItems");
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
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Slot", b =>
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

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Slots");
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
                        .IsRequired()
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
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.UserCharacter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_in(md5(random()::text || clock_timestamp()::text)::cstring)");

                    b.Property<Guid>("CharacterId")
                        .HasColumnType("uuid");

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

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.HasIndex("UserId");

                    b.ToTable("UserCharacters");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.UserGame", b =>
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

                    b.Property<Guid>("GameId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ModifiedByUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("UserId");

                    b.ToTable("UserGames");
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

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Bag", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Game", "Game")
                        .WithMany("Bags")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.BagItem", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Bag", "Bag")
                        .WithMany("BagItems")
                        .HasForeignKey("BagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sindie.ApiService.Core.Entities.Item", "Item")
                        .WithMany("BagItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bag");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Body", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Character", "Character")
                        .WithMany("Bodies")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sindie.ApiService.Core.Entities.Slot", "Slot")
                        .WithMany("Bodies")
                        .HasForeignKey("SlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("Slot");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.BodyItem", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Character", "Character")
                        .WithMany("BodyItems")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sindie.ApiService.Core.Entities.Item", "Item")
                        .WithMany("BodyItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Character", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Bag", "Bag")
                        .WithOne("Character")
                        .HasForeignKey("Sindie.ApiService.Core.Entities.Character", "BagId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("Sindie.ApiService.Core.Entities.Game", "Game")
                        .WithMany("Characters")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sindie.ApiService.Core.Entities.UserCharacter", "UserCharacter")
                        .WithOne("ActivateCharacter")
                        .HasForeignKey("Sindie.ApiService.Core.Entities.Character", "UserActivateId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Bag");

                    b.Navigation("Game");

                    b.Navigation("UserCharacter");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.CharacterParameter", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Character", "Character")
                        .WithMany("CharacterParameters")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sindie.ApiService.Core.Entities.Parameter", "Parameter")
                        .WithMany("CharacterParameters")
                        .HasForeignKey("ParameterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("Parameter");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Item", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Slot", "Slot")
                        .WithMany("Items")
                        .HasForeignKey("SlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Slot");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Parameter", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Game", "Game")
                        .WithMany("Parameters")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.ParameterItem", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Item", "Item")
                        .WithMany("ParameterItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sindie.ApiService.Core.Entities.Parameter", "Parameter")
                        .WithMany("ParameterItems")
                        .HasForeignKey("ParameterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Parameter");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Slot", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Game", "Game")
                        .WithMany("Slots")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
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

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.UserCharacter", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Character", "Character")
                        .WithMany("UserCharacters")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sindie.ApiService.Core.Entities.User", "User")
                        .WithMany("UserCharacters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Character");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.UserGame", b =>
                {
                    b.HasOne("Sindie.ApiService.Core.Entities.Game", "Game")
                        .WithMany("UserGames")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sindie.ApiService.Core.Entities.User", "User")
                        .WithMany("UserGames")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

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

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Bag", b =>
                {
                    b.Navigation("BagItems");

                    b.Navigation("Character");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Character", b =>
                {
                    b.Navigation("Bodies");

                    b.Navigation("BodyItems");

                    b.Navigation("CharacterParameters");

                    b.Navigation("UserCharacters");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Game", b =>
                {
                    b.Navigation("Bags");

                    b.Navigation("Characters");

                    b.Navigation("Parameters");

                    b.Navigation("Slots");

                    b.Navigation("UserGames");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Item", b =>
                {
                    b.Navigation("BagItems");

                    b.Navigation("BodyItems");

                    b.Navigation("ParameterItems");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Parameter", b =>
                {
                    b.Navigation("CharacterParameters");

                    b.Navigation("ParameterItems");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.Slot", b =>
                {
                    b.Navigation("Bodies");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.User", b =>
                {
                    b.Navigation("UserAccounts");

                    b.Navigation("UserCharacters");

                    b.Navigation("UserGames");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Sindie.ApiService.Core.Entities.UserCharacter", b =>
                {
                    b.Navigation("ActivateCharacter");
                });
#pragma warning restore 612, 618
        }
    }
}