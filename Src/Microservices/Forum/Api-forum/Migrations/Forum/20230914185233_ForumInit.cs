using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api_forum.Migrations.Forum
{
    /// <inheritdoc />
    public partial class ForumInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cabinet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InternalPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Division = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LatestLoginOnForum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForumAccountType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumAccountType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Karma = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    UpdatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    TotalPostCounter = table.Column<int>(type: "INTEGER", nullable: true, defaultValue: 0),
                    AvatarImgSrc = table.Column<string>(type: "NVARCHAR", nullable: true),
                    AppUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumUser_AppUser_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ForumAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Ip = table.Column<string>(type: "NVARCHAR", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumAccount_ForumAccountType_Id",
                        column: x => x.AccountTypeId,
                        principalTable: "ForumAccountType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ForumAccount_ForumUser_Id",
                        column: x => x.Id,
                        principalTable: "ForumUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    TotalTopics = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalForums = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    ForumUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumCategory_ForumUser_Id",
                        column: x => x.ForumUserId,
                        principalTable: "ForumUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ForumBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForumTitle = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    ForumSubTitle = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    TotalTopics = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalViews = table.Column<int>(type: "INTEGER", nullable: false),
                    ForumCategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    ForumUserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumBase_ForumUser_Id",
                        column: x => x.ForumUserId,
                        principalTable: "ForumUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ForumCategory_ForumBase_Id",
                        column: x => x.ForumCategoryId,
                        principalTable: "ForumCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ForumTopic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    TotalPosts = table.Column<int>(type: "int", nullable: false),
                    TotalViews = table.Column<int>(type: "INTEGER", nullable: false),
                    ForumUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ForumBaseId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTopic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumBase_ForumTopic_ForumBaseId",
                        column: x => x.ForumBaseId,
                        principalTable: "ForumBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ForumTopic_ForumUser_Id",
                        column: x => x.ForumUserId,
                        principalTable: "ForumUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ForumPost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostText = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: true),
                    Likes = table.Column<int>(type: "INTEGER", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "Date", nullable: true),
                    ForumTopicId = table.Column<int>(type: "INTEGER", nullable: true),
                    ForumUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ForumFileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumPost_ForumUser_Id",
                        column: x => x.ForumUserId,
                        principalTable: "ForumUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ForumTopic_ForumPost_ForumTopicId",
                        column: x => x.ForumTopicId,
                        principalTable: "ForumTopic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ForumTopicCounter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostCounter = table.Column<int>(type: "INTEGER", nullable: true),
                    ForumTopicId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTopicCounter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumTopic_ForumTopicCounter_Id",
                        column: x => x.ForumTopicId,
                        principalTable: "ForumTopic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ForumFile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    Path = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    ForumUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    ForumPostId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumPost_ForumFile_Id",
                        column: x => x.ForumPostId,
                        principalTable: "ForumPost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ForumUser_ForumFile_ForumUserId",
                        column: x => x.ForumUserId,
                        principalTable: "ForumUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, null, "USER", "USER" },
                    { 2, null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BirthDate", "Cabinet", "Company", "ConcurrencyStamp", "CreatedAt", "Division", "Email", "EmailConfirmed", "FirstName", "InternalPhone", "LastName", "LatestLoginOnForum", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Position", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 1, 0, "0", "0", "My company", "7385ca32-aff9-4f49-ad16-7f64b4fa2754", new DateTime(2023, 9, 14, 21, 52, 33, 352, DateTimeKind.Local).AddTicks(6433), "My division", "Admin@admin.by", false, "System", "0", "Admin", null, false, null, "ADMIN@ADMIN.BY", "ADMIN", "AQAAAAIAAYagAAAAEJmQy9UwxkODjbb/iQlo7ezznBC5omr0sEhFEoTgafpAxZZRFsyVCFG8NXKSc2SGJA==", "0", false, "Administrator", "FGXU4FIM2LMJZFDJD3YCUQEHQRZY4GSS", false, "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "ForumUser",
                columns: new[] { "Id", "AppUserId", "AvatarImgSrc", "TotalPostCounter", "UpdatedAt" },
                values: new object[] { 1, 1, null, 10, null });

            migrationBuilder.InsertData(
                table: "ForumCategory",
                columns: new[] { "Id", "CreatedAt", "ForumUserId", "Name", "TotalForums", "TotalTopics", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(1550), 1, "Test category 1", 1, 4, null },
                    { 2, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(1553), 1, "Test category 2", 5, 4, null },
                    { 3, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(1555), 1, "Test category 3", 0, 0, null },
                    { 4, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(1556), 1, "Test category 4", 0, 0, null },
                    { 5, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(1558), 1, "Test category 5", 0, 0, null },
                    { 6, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(1560), 1, "Test category 6", 0, 0, null }
                });

            migrationBuilder.InsertData(
                table: "ForumBase",
                columns: new[] { "Id", "CreatedAt", "ForumCategoryId", "ForumSubTitle", "ForumTitle", "ForumUserId", "TotalTopics", "TotalViews", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(6152), 1, "Test forum subtitle 1", "Test forum title 1", 1, null, 0, null },
                    { 2, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(6155), 2, "Test forum subtitle 2", "Test forum title 2", 1, null, 0, null },
                    { 3, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(6157), 2, "Test forum subtitle 3", "Test forum title 3", 1, null, 0, null },
                    { 4, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(6159), 2, "Test forum subtitle 4", "Test forum title 4", 1, null, 0, null },
                    { 5, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(6161), 2, "Test forum subtitle 5", "Test forum title 5", 1, null, 0, null },
                    { 6, new DateTime(2023, 9, 14, 21, 52, 33, 354, DateTimeKind.Local).AddTicks(6162), 2, "Test forum subtitle 6", "Test forum title 6", 1, null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "ForumTopic",
                columns: new[] { "Id", "CreatedAt", "ForumBaseId", "ForumUserId", "Name", "TotalPosts", "TotalViews", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(734), 1, 1, "Test forum topic 1", 0, 0, null },
                    { 2, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(742), 2, 1, "Test forum topic 2", 0, 0, null },
                    { 3, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(743), 2, 1, "Test forum topic 3", 0, 0, null },
                    { 4, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(745), 2, 1, "Test forum topic 4", 0, 0, null },
                    { 5, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(746), 2, 1, "Test forum topic 5", 0, 0, null },
                    { 6, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(747), 1, 1, "Test forum topic 1a", 0, 0, null },
                    { 7, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(749), 1, 1, "Test forum topic 1b", 0, 0, null },
                    { 8, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(750), 1, 1, "Test forum topic 1c", 0, 0, null }
                });

            migrationBuilder.InsertData(
                table: "ForumPost",
                columns: new[] { "Id", "CreatedAt", "ForumFileId", "ForumTopicId", "ForumUserId", "Likes", "PostText", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(3990), 0, 1, 1, 0, "1111111111111111111111", null },
                    { 2, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(3996), 0, 2, 1, 0, null, null },
                    { 3, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(3997), 0, 2, 1, 0, null, null },
                    { 4, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(3998), 0, 2, 1, 0, null, null },
                    { 5, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(4000), 0, 2, 1, 0, null, null },
                    { 6, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(4001), 0, 1, 1, 0, "222222222222222222", null },
                    { 7, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(4002), 0, 1, 1, 0, "333333333333333", null },
                    { 8, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(4004), 0, 1, 1, 0, "44444444444444", null },
                    { 9, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(4005), 0, 1, 1, 0, "555555555555555", null },
                    { 10, new DateTime(2023, 9, 14, 21, 52, 33, 355, DateTimeKind.Local).AddTicks(4006), 0, 1, 1, 0, "666666666666666", null }
                });

            migrationBuilder.InsertData(
                table: "ForumTopicCounter",
                columns: new[] { "Id", "ForumTopicId", "PostCounter" },
                values: new object[,]
                {
                    { 1, 1, 6 },
                    { 2, 2, 4 },
                    { 3, 3, 0 },
                    { 4, 4, 0 },
                    { 5, 5, 0 },
                    { 6, 6, 0 },
                    { 7, 7, 0 },
                    { 8, 8, 0 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ForumAccount_AccountTypeId",
                table: "ForumAccount",
                column: "AccountTypeId",
                unique: true,
                filter: "[AccountTypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ForumBase_ForumCategoryId",
                table: "ForumBase",
                column: "ForumCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumBase_ForumUserId",
                table: "ForumBase",
                column: "ForumUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumCategory_ForumUserId",
                table: "ForumCategory",
                column: "ForumUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumFile_ForumPostId",
                table: "ForumFile",
                column: "ForumPostId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumFile_ForumUserId",
                table: "ForumFile",
                column: "ForumUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPost_ForumTopicId",
                table: "ForumPost",
                column: "ForumTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPost_ForumUserId",
                table: "ForumPost",
                column: "ForumUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopic_ForumBaseId",
                table: "ForumTopic",
                column: "ForumBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopic_ForumUserId",
                table: "ForumTopic",
                column: "ForumUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopicCounter_ForumTopicId",
                table: "ForumTopicCounter",
                column: "ForumTopicId",
                unique: true,
                filter: "[ForumTopicId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ForumUser_TotalPostCounter",
                table: "ForumUser",
                column: "TotalPostCounter");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ForumAccount");

            migrationBuilder.DropTable(
                name: "ForumFile");

            migrationBuilder.DropTable(
                name: "ForumTopicCounter");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ForumAccountType");

            migrationBuilder.DropTable(
                name: "ForumPost");

            migrationBuilder.DropTable(
                name: "ForumTopic");

            migrationBuilder.DropTable(
                name: "ForumBase");

            migrationBuilder.DropTable(
                name: "ForumCategory");

            migrationBuilder.DropTable(
                name: "ForumUser");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
