using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class TablolariOlustur : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalonHizmetleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sure = table.Column<int>(type: "int", nullable: false),
                    Ucret = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalonHizmetleri", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Egitmenler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdSoyad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UzmanlikAlani = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalonHizmetiId = table.Column<int>(type: "int", nullable: false),
                    CalismaSaatleri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Egitmenler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Egitmenler_SalonHizmetleri_SalonHizmetiId",
                        column: x => x.SalonHizmetiId,
                        principalTable: "SalonHizmetleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Randevular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tarih = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Saat = table.Column<TimeSpan>(type: "time", nullable: false),
                    OlusturulmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UyeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EgitmenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Randevular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Randevular_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Randevular_Egitmenler_EgitmenId",
                        column: x => x.EgitmenId,
                        principalTable: "Egitmenler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Egitmenler_SalonHizmetiId",
                table: "Egitmenler",
                column: "SalonHizmetiId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_EgitmenId",
                table: "Randevular",
                column: "EgitmenId");

            migrationBuilder.CreateIndex(
                name: "IX_Randevular_UyeId",
                table: "Randevular",
                column: "UyeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Randevular");

            migrationBuilder.DropTable(
                name: "Egitmenler");

            migrationBuilder.DropTable(
                name: "SalonHizmetleri");
        }
    }
}
