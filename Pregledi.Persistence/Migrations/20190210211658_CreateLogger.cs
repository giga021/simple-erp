using Microsoft.EntityFrameworkCore.Migrations;

namespace Pregledi.Persistence.Migrations
{
    public partial class CreateLogger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"CREATE TABLE IF NOT EXISTS `log` (
			  `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
			  `entered_date` timestamp NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
			  `log_application` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
			  `log_date` datetime DEFAULT NULL,
			  `log_level` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
			  `log_logger` varchar(1000) COLLATE utf8_unicode_ci DEFAULT NULL,
			  `log_message` varchar(1000) COLLATE utf8_unicode_ci DEFAULT NULL,
			  `log_machine_name` varchar(1000) COLLATE utf8_unicode_ci DEFAULT NULL,
			  `log_user_name` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
			  `log_call_site` varchar(1000) COLLATE utf8_unicode_ci DEFAULT NULL,
			  `log_thread` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
			  `log_exception` varchar(8000) COLLATE utf8_unicode_ci DEFAULT NULL,
			  `log_stacktrace` varchar(8000) COLLATE utf8_unicode_ci DEFAULT NULL,
			  PRIMARY KEY (`Id`)) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
