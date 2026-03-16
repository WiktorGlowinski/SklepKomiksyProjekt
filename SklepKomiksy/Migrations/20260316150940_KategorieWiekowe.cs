using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SklepKomiksy.Migrations
{
    /// <inheritdoc />
    public partial class KategorieWiekowe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "18+",
                table: "kategorie_wiekowe");

            migrationBuilder.AddColumn<string>(
                name: "nazwa",
                table: "kategorie_wiekowe",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nazwa",
                table: "kategorie_wiekowe");

            migrationBuilder.AddColumn<bool>(
                name: "18+",
                table: "kategorie_wiekowe",
                type: "tinyint(1)",
                nullable: true);
        }
    }
}
