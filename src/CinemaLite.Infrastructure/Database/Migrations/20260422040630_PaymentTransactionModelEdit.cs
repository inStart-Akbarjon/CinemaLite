using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaLite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PaymentTransactionModelEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "PaymentTransactions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "PaymentTransactions");
        }
    }
}
