using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShootyGameAPI.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    PlayerTag = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Money = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "WeaponTypes",
                columns: table => new
                {
                    WeaponTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    EquipmentSlot = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeaponTypes", x => x.WeaponTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    FriendRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequesterId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    ResponseAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.FriendRequestId);
                    table.ForeignKey(
                        name: "FK_FriendRequests_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_FriendRequests_Users_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    User1Id = table.Column<int>(type: "int", nullable: false),
                    User2Id = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => new { x.User1Id, x.User2Id });
                    table.ForeignKey(
                        name: "FK_Friends_Users_User1Id",
                        column: x => x.User1Id,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Friends_Users_User2Id",
                        column: x => x.User2Id,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    ScoreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ScoreValue = table.Column<int>(type: "int", nullable: false),
                    AverageAccuracy = table.Column<float>(type: "real", nullable: false),
                    RoundTime = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.ScoreId);
                    table.ForeignKey(
                        name: "FK_Scores_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    WeaponId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeaponTypeId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    ReloadSpeed = table.Column<float>(type: "real", nullable: false),
                    MagSize = table.Column<int>(type: "int", nullable: false),
                    FireRate = table.Column<int>(type: "int", nullable: false),
                    FireMode = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.WeaponId);
                    table.ForeignKey(
                        name: "FK_Weapons_WeaponTypes_WeaponTypeId",
                        column: x => x.WeaponTypeId,
                        principalTable: "WeaponTypes",
                        principalColumn: "WeaponTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserWeapons",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WeaponId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWeapons", x => new { x.UserId, x.WeaponId });
                    table.ForeignKey(
                        name: "FK_UserWeapons_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserWeapons_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "WeaponId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "IsDeleted", "Money", "PasswordHash", "PlayerTag", "Role", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 6, 14, 21, 42, 70, DateTimeKind.Unspecified), "admin@mail.com", false, 0, "AQAAAAIAAYagAAAAEJMTFuO/fgInS4QHEQaSUkszZ3nuDWYQ0H4BcKRE94iHmvahKA+0Eueh5wgQKIbYuw==", "TestUser#7f3e4779", 0, "TestUser" },
                    { 2, new DateTime(2025, 3, 6, 14, 22, 3, 780, DateTimeKind.Unspecified), "user@mail.com", false, 0, "AQAAAAIAAYagAAAAEP3n76UekjMkwna2ALIGJPoOAt/wZ8MrGQohB4/muBc1z2G4MpOPE7+wKt/JzoHFSw==", "TestUser#29818102", 1, "TestUser" }
                });

            migrationBuilder.InsertData(
                table: "WeaponTypes",
                columns: new[] { "WeaponTypeId", "CreatedAt", "EquipmentSlot", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 11, 11, 36, 37, 100, DateTimeKind.Unspecified), 1, false, "Pistol" },
                    { 2, new DateTime(2025, 3, 11, 11, 36, 37, 150, DateTimeKind.Unspecified), 1, false, "Machine Pistol" },
                    { 3, new DateTime(2025, 3, 11, 11, 36, 37, 200, DateTimeKind.Unspecified), 0, false, "Assault Rifle" },
                    { 4, new DateTime(2025, 3, 11, 11, 36, 37, 250, DateTimeKind.Unspecified), 0, false, "Marksman Rifle" }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "WeaponId", "CreatedAt", "FireMode", "FireRate", "IsDeleted", "MagSize", "Name", "Price", "ReloadSpeed", "WeaponTypeId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 3, 11, 12, 0, 0, 0, DateTimeKind.Unspecified), 0, 600, false, 15, "M9", 0, 0.95f, 1 },
                    { 2, new DateTime(2025, 3, 11, 12, 2, 0, 0, DateTimeKind.Unspecified), 1, 1200, false, 18, "Tec9", 400, 1.05f, 2 },
                    { 3, new DateTime(2025, 3, 11, 12, 4, 0, 0, DateTimeKind.Unspecified), 1, 750, false, 30, "G36", 0, 1.9f, 3 },
                    { 4, new DateTime(2025, 3, 11, 12, 6, 0, 0, DateTimeKind.Unspecified), 0, 300, false, 15, "Scar-H", 800, 1.82f, 4 }
                });

            migrationBuilder.InsertData(
                table: "UserWeapons",
                columns: new[] { "UserId", "WeaponId", "CreatedAt", "IsDeleted" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 3, 12, 12, 10, 0, 0, DateTimeKind.Unspecified), false },
                    { 1, 3, new DateTime(2025, 3, 12, 12, 12, 0, 0, DateTimeKind.Unspecified), false },
                    { 2, 1, new DateTime(2025, 3, 12, 12, 14, 0, 0, DateTimeKind.Unspecified), false },
                    { 2, 3, new DateTime(2025, 3, 12, 12, 16, 0, 0, DateTimeKind.Unspecified), false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_RequesterId",
                table: "FriendRequests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_User2Id",
                table: "Friends",
                column: "User2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_UserId",
                table: "Scores",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PlayerTag",
                table: "Users",
                column: "PlayerTag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWeapons_WeaponId",
                table: "UserWeapons",
                column: "WeaponId");

            migrationBuilder.CreateIndex(
                name: "IX_Weapons_WeaponTypeId",
                table: "Weapons",
                column: "WeaponTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "UserWeapons");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "WeaponTypes");
        }
    }
}
