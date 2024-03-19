using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IT_DeskServer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ticketUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketDetails_Users_AppUserId",
                table: "TicketDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_AppUserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TicketDetails");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppUserId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AppUserId",
                table: "TicketDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketDetails_Users_AppUserId",
                table: "TicketDetails",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_AppUserId",
                table: "Tickets",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketDetails_Users_AppUserId",
                table: "TicketDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Users_AppUserId",
                table: "Tickets");

            migrationBuilder.AlterColumn<Guid>(
                name: "AppUserId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "AppUserId",
                table: "TicketDetails",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "TicketDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_TicketDetails_Users_AppUserId",
                table: "TicketDetails",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Users_AppUserId",
                table: "Tickets",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
