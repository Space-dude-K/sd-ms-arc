using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_fsc.Migrations
{
    /// <inheritdoc />
    public partial class FscInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ip = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    Disk = table.Column<string>(type: "NVARCHAR(256)", maxLength: 256, nullable: false),
                    TotalSpace = table.Column<float>(type: "FLOAT", maxLength: 256, nullable: true),
                    FreeSpace = table.Column<float>(type: "FLOAT", maxLength: 256, nullable: true),
                    DateTime = table.Column<DateTime>(type: "DATETIME", maxLength: 256, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Device");
        }
    }
}
