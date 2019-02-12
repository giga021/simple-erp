using Microsoft.EntityFrameworkCore.Migrations;

namespace Pregledi.Persistence.Migrations
{
    public partial class CreateProcIzracunajKarticu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"CREATE DEFINER=`root`@`%` PROCEDURE `izracunaj_karticu_konta`()
				BEGIN
					SET @csum := 0;
					SET @idKonto := 0;
					UPDATE kartica_konta
					SET SaldoKumulativno = (IF(@idKonto <> IdKonto and @idKonto := IdKonto, @csum := Saldo, @csum := @csum + Saldo))
					ORDER BY IdKonto, DatumNaloga, DatumKnjizenja, Created;
				END;");
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
