﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShootyGameAPI.Database;

#nullable disable

namespace ShootyGameAPI.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.Friend", b =>
                {
                    b.Property<int>("User1Id")
                        .HasColumnType("int");

                    b.Property<int>("User2Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("User1Id", "User2Id");

                    b.HasIndex("User2Id");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.FriendRequest", b =>
                {
                    b.Property<int>("FriendRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FriendRequestId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<int>("ReceiverId")
                        .HasColumnType("int");

                    b.Property<int>("RequesterId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ResponseAt")
                        .HasColumnType("datetime");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("FriendRequestId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("RequesterId");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.Score", b =>
                {
                    b.Property<int>("ScoreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScoreId"));

                    b.Property<float>("AverageAccuracy")
                        .HasColumnType("real");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<float>("RoundTime")
                        .HasColumnType("real");

                    b.Property<int>("ScoreValue")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ScoreId");

                    b.HasIndex("UserId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Money")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PlayerTag")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("PlayerTag")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            CreatedAt = new DateTime(2025, 3, 6, 14, 21, 42, 70, DateTimeKind.Unspecified),
                            Email = "admin@mail.com",
                            IsDeleted = false,
                            Money = 0,
                            PasswordHash = "AQAAAAIAAYagAAAAEJMTFuO/fgInS4QHEQaSUkszZ3nuDWYQ0H4BcKRE94iHmvahKA+0Eueh5wgQKIbYuw==",
                            PlayerTag = "TestUser#7f3e4779",
                            Role = 0,
                            UserName = "TestUser"
                        },
                        new
                        {
                            UserId = 2,
                            CreatedAt = new DateTime(2025, 3, 6, 14, 22, 3, 780, DateTimeKind.Unspecified),
                            Email = "user@mail.com",
                            IsDeleted = false,
                            Money = 0,
                            PasswordHash = "AQAAAAIAAYagAAAAEP3n76UekjMkwna2ALIGJPoOAt/wZ8MrGQohB4/muBc1z2G4MpOPE7+wKt/JzoHFSw==",
                            PlayerTag = "TestUser#29818102",
                            Role = 1,
                            UserName = "TestUser"
                        });
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.UserWeapon", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WeaponId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "WeaponId");

                    b.HasIndex("WeaponId");

                    b.ToTable("UserWeapons");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            WeaponId = 1,
                            CreatedAt = new DateTime(2025, 3, 12, 12, 10, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false
                        },
                        new
                        {
                            UserId = 1,
                            WeaponId = 3,
                            CreatedAt = new DateTime(2025, 3, 12, 12, 12, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false
                        },
                        new
                        {
                            UserId = 2,
                            WeaponId = 1,
                            CreatedAt = new DateTime(2025, 3, 12, 12, 14, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false
                        },
                        new
                        {
                            UserId = 2,
                            WeaponId = 3,
                            CreatedAt = new DateTime(2025, 3, 12, 12, 16, 0, 0, DateTimeKind.Unspecified),
                            IsDeleted = false
                        });
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.Weapon", b =>
                {
                    b.Property<int>("WeaponId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WeaponId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<int>("FireMode")
                        .HasColumnType("int");

                    b.Property<int>("FireRate")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("MagSize")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<float>("ReloadSpeed")
                        .HasColumnType("real");

                    b.Property<int>("WeaponTypeId")
                        .HasColumnType("int");

                    b.HasKey("WeaponId");

                    b.HasIndex("WeaponTypeId");

                    b.ToTable("Weapons");

                    b.HasData(
                        new
                        {
                            WeaponId = 1,
                            CreatedAt = new DateTime(2025, 3, 11, 12, 0, 0, 0, DateTimeKind.Unspecified),
                            FireMode = 0,
                            FireRate = 600,
                            IsDeleted = false,
                            MagSize = 15,
                            Name = "M9",
                            Price = 0,
                            ReloadSpeed = 0.95f,
                            WeaponTypeId = 1
                        },
                        new
                        {
                            WeaponId = 2,
                            CreatedAt = new DateTime(2025, 3, 11, 12, 2, 0, 0, DateTimeKind.Unspecified),
                            FireMode = 1,
                            FireRate = 1200,
                            IsDeleted = false,
                            MagSize = 18,
                            Name = "Tec9",
                            Price = 400,
                            ReloadSpeed = 1.05f,
                            WeaponTypeId = 2
                        },
                        new
                        {
                            WeaponId = 3,
                            CreatedAt = new DateTime(2025, 3, 11, 12, 4, 0, 0, DateTimeKind.Unspecified),
                            FireMode = 1,
                            FireRate = 750,
                            IsDeleted = false,
                            MagSize = 30,
                            Name = "G36",
                            Price = 0,
                            ReloadSpeed = 1.9f,
                            WeaponTypeId = 3
                        },
                        new
                        {
                            WeaponId = 4,
                            CreatedAt = new DateTime(2025, 3, 11, 12, 6, 0, 0, DateTimeKind.Unspecified),
                            FireMode = 0,
                            FireRate = 300,
                            IsDeleted = false,
                            MagSize = 15,
                            Name = "Scar-H",
                            Price = 800,
                            ReloadSpeed = 1.82f,
                            WeaponTypeId = 4
                        });
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.WeaponType", b =>
                {
                    b.Property<int>("WeaponTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WeaponTypeId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<int>("EquipmentSlot")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("WeaponTypeId");

                    b.ToTable("WeaponTypes");

                    b.HasData(
                        new
                        {
                            WeaponTypeId = 1,
                            CreatedAt = new DateTime(2025, 3, 11, 11, 36, 37, 100, DateTimeKind.Unspecified),
                            EquipmentSlot = 1,
                            IsDeleted = false,
                            Name = "Pistol"
                        },
                        new
                        {
                            WeaponTypeId = 2,
                            CreatedAt = new DateTime(2025, 3, 11, 11, 36, 37, 150, DateTimeKind.Unspecified),
                            EquipmentSlot = 1,
                            IsDeleted = false,
                            Name = "Machine Pistol"
                        },
                        new
                        {
                            WeaponTypeId = 3,
                            CreatedAt = new DateTime(2025, 3, 11, 11, 36, 37, 200, DateTimeKind.Unspecified),
                            EquipmentSlot = 0,
                            IsDeleted = false,
                            Name = "Assault Rifle"
                        },
                        new
                        {
                            WeaponTypeId = 4,
                            CreatedAt = new DateTime(2025, 3, 11, 11, 36, 37, 250, DateTimeKind.Unspecified),
                            EquipmentSlot = 0,
                            IsDeleted = false,
                            Name = "Marksman Rifle"
                        });
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.Friend", b =>
                {
                    b.HasOne("ShootyGameAPI.Database.Entities.User", "User1")
                        .WithMany("Friends1")
                        .HasForeignKey("User1Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ShootyGameAPI.Database.Entities.User", "User2")
                        .WithMany("Friends2")
                        .HasForeignKey("User2Id")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User1");

                    b.Navigation("User2");
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.FriendRequest", b =>
                {
                    b.HasOne("ShootyGameAPI.Database.Entities.User", "Receiver")
                        .WithMany("FriendRequests2")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("ShootyGameAPI.Database.Entities.User", "Requester")
                        .WithMany("FriendRequests1")
                        .HasForeignKey("RequesterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Requester");
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.Score", b =>
                {
                    b.HasOne("ShootyGameAPI.Database.Entities.User", "User")
                        .WithMany("Scores")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.UserWeapon", b =>
                {
                    b.HasOne("ShootyGameAPI.Database.Entities.User", "User")
                        .WithMany("UserWeapons")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShootyGameAPI.Database.Entities.Weapon", "Weapon")
                        .WithMany("UserWeapons")
                        .HasForeignKey("WeaponId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Weapon");
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.Weapon", b =>
                {
                    b.HasOne("ShootyGameAPI.Database.Entities.WeaponType", "WeaponType")
                        .WithMany("Weapons")
                        .HasForeignKey("WeaponTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WeaponType");
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.User", b =>
                {
                    b.Navigation("FriendRequests1");

                    b.Navigation("FriendRequests2");

                    b.Navigation("Friends1");

                    b.Navigation("Friends2");

                    b.Navigation("Scores");

                    b.Navigation("UserWeapons");
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.Weapon", b =>
                {
                    b.Navigation("UserWeapons");
                });

            modelBuilder.Entity("ShootyGameAPI.Database.Entities.WeaponType", b =>
                {
                    b.Navigation("Weapons");
                });
#pragma warning restore 612, 618
        }
    }
}
