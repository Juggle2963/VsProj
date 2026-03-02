using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScanDirectories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastScanDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScanDirectories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MusicFiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Artist = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Album = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Genre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "int", nullable: true),
                    Track = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: true),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Bitrate = table.Column<int>(type: "int", nullable: true),
                    ScanDirectoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MusicFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MusicFiles_ScanDirectories_ScanDirectoryId",
                        column: x => x.ScanDirectoryId,
                        principalTable: "ScanDirectories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MusicFiles_FilePath",
                table: "MusicFiles",
                column: "FilePath",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MusicFiles_ScanDirectoryId",
                table: "MusicFiles",
                column: "ScanDirectoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScanDirectories_Path",
                table: "ScanDirectories",
                column: "Path",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MusicFiles");

            migrationBuilder.DropTable(
                name: "ScanDirectories");
        }
    }
}
