using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InvestOA.Core.Migrations
{
    public partial class updatehistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeOfSelling",
                table: "Histories",
                newName: "TimeOfTransaction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeOfTransaction",
                table: "Histories",
                newName: "TimeOfSelling");
        }
    }
}
