using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaLite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTicketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tickets");
        }
    }
}
