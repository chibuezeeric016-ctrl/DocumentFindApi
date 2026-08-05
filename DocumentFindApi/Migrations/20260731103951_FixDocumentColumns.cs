using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocumentFindApi.Migrations
{
    /// <inheritdoc />
    public partial class FixDocumentColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Documents",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FileSize",
                table: "Documents",
                newName: "FileSizeBytes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Documents",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "FileSizeBytes",
                table: "Documents",
                newName: "FileSize");
        }
    }
}
