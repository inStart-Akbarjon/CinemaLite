using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaLite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserEmailColumnToTicketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "Tickets",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "Tickets");
        }
    }
}
