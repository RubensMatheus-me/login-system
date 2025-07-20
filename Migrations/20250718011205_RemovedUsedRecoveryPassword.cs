using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace login_system.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUsedRecoveryPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Used",
                table: "RecoveryPassword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Used",
                table: "RecoveryPassword",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
