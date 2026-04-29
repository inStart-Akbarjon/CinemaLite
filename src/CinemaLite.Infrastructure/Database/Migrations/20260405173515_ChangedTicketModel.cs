using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaLite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedTicketModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Cart_CartId",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "CinemaName",
                table: "SeatReservations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePaid",
                table: "SeatReservations",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "SeatReservations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Cart_CartId",
                table: "Tickets",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Cart_CartId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CinemaName",
                table: "SeatReservations");

            migrationBuilder.DropColumn(
                name: "PricePaid",
                table: "SeatReservations");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "SeatReservations");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Cart_CartId",
                table: "Tickets",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
