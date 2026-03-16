using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SklepKomiksy.Migrations
{
    /// <inheritdoc />
    public partial class WieleDoWielu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutorProdukt",
                columns: table => new
                {
                    AutorzyId = table.Column<long>(type: "bigint", nullable: false),
                    ProduktyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorProdukt", x => new { x.AutorzyId, x.ProduktyId });
                    table.ForeignKey(
                        name: "FK_AutorProdukt_autorzy_AutorzyId",
                        column: x => x.AutorzyId,
                        principalTable: "autorzy",
                        principalColumn: "id_autora",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutorProdukt_produkty_ProduktyId",
                        column: x => x.ProduktyId,
                        principalTable: "produkty",
                        principalColumn: "id_produktu",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GatunekProdukt",
                columns: table => new
                {
                    GatunkiId = table.Column<long>(type: "bigint", nullable: false),
                    ProduktyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatunekProdukt", x => new { x.GatunkiId, x.ProduktyId });
                    table.ForeignKey(
                        name: "FK_GatunekProdukt_gatunki_GatunkiId",
                        column: x => x.GatunkiId,
                        principalTable: "gatunki",
                        principalColumn: "id_gatunku",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GatunekProdukt_produkty_ProduktyId",
                        column: x => x.ProduktyId,
                        principalTable: "produkty",
                        principalColumn: "id_produktu",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AutorProdukt_ProduktyId",
                table: "AutorProdukt",
                column: "ProduktyId");

            migrationBuilder.CreateIndex(
                name: "IX_GatunekProdukt_ProduktyId",
                table: "GatunekProdukt",
                column: "ProduktyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutorProdukt");

            migrationBuilder.DropTable(
                name: "GatunekProdukt");
        }
    }
}
