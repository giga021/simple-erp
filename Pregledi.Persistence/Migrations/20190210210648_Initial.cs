using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pregledi.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "kartica_konta",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdNaloga = table.Column<Guid>(nullable: false),
                    DatumNaloga = table.Column<DateTime>(type: "date", nullable: false),
                    TipNaloga = table.Column<string>(nullable: true),
                    DatumKnjizenja = table.Column<DateTime>(type: "date", nullable: false),
                    IdKonto = table.Column<long>(nullable: false),
                    Konto = table.Column<string>(maxLength: 10, nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    Duguje = table.Column<decimal>(nullable: false),
                    Potrazuje = table.Column<decimal>(nullable: false),
                    Saldo = table.Column<decimal>(nullable: false),
                    SaldoKumulativno = table.Column<decimal>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Version = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kartica_konta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "konto",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Sifra = table.Column<string>(nullable: true),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_konto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "nalog_form",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Datum = table.Column<DateTime>(type: "date", nullable: false),
                    IdTip = table.Column<long>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    Version = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nalog_form", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "nalog_glavna_knjiga",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Datum = table.Column<DateTime>(type: "date", nullable: false),
                    TipNaziv = table.Column<string>(nullable: true),
                    Opis = table.Column<string>(nullable: true),
                    BrojStavki = table.Column<int>(nullable: false),
                    UkupnoDuguje = table.Column<decimal>(nullable: false),
                    UkupnoPotrazuje = table.Column<decimal>(nullable: false),
                    UkupnoSaldo = table.Column<decimal>(nullable: false),
                    Zakljucan = table.Column<bool>(nullable: false),
                    Version = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_nalog_glavna_knjiga", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "processed_event",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OriginalStream = table.Column<string>(nullable: true),
                    Stream = table.Column<string>(maxLength: 255, nullable: false),
                    Checkpoint = table.Column<long>(nullable: true),
                    CommitPosition = table.Column<long>(nullable: true),
                    PreparePosition = table.Column<long>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processed_event", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tip_naloga",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tip_naloga", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stavka_form",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IdNaloga = table.Column<Guid>(nullable: false),
                    IdKonto = table.Column<long>(nullable: false),
                    Konto = table.Column<string>(maxLength: 10, nullable: false),
                    DatumKnjizenja = table.Column<DateTime>(type: "date", nullable: false),
                    Duguje = table.Column<decimal>(nullable: false),
                    Potrazuje = table.Column<decimal>(nullable: false),
                    Opis = table.Column<string>(nullable: true),
                    Version = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stavka_form", x => x.Id);
                    table.ForeignKey(
                        name: "FK_stavka_form_nalog_form_IdNaloga",
                        column: x => x.IdNaloga,
                        principalTable: "nalog_form",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_stavka_form_IdNaloga",
                table: "stavka_form",
                column: "IdNaloga");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "kartica_konta");

            migrationBuilder.DropTable(
                name: "konto");

            migrationBuilder.DropTable(
                name: "nalog_glavna_knjiga");

            migrationBuilder.DropTable(
                name: "processed_event");

            migrationBuilder.DropTable(
                name: "stavka_form");

            migrationBuilder.DropTable(
                name: "tip_naloga");

            migrationBuilder.DropTable(
                name: "nalog_form");
        }
    }
}
