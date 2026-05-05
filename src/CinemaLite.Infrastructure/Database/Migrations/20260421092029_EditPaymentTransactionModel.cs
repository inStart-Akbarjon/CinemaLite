using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaLite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditPaymentTransactionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GatewayResponse",
                table: "PaymentTransactions");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "PaymentTransactions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "PaymentTransactions");

            migrationBuilder.AddColumn<string>(
                name: "GatewayResponse",
                table: "PaymentTransactions",
                type: "text",
                nullable: true);
        }
    }
}
