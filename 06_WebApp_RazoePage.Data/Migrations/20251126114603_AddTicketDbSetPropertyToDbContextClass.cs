using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _06_WebApp_RazoePage.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketDbSetPropertyToDbContextClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Customers_CustomerId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Movies_MovieId",
                table: "Ticket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket");

            migrationBuilder.RenameTable(
                name: "Ticket",
                newName: "Tickets");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_MovieId",
                table: "Tickets",
                newName: "IX_Tickets_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_CustomerId",
                table: "Tickets",
                newName: "IX_Tickets_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Customers_CustomerId",
                table: "Tickets",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Movies_MovieId",
                table: "Tickets",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Customers_CustomerId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Movies_MovieId",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tickets",
                table: "Tickets");

            migrationBuilder.RenameTable(
                name: "Tickets",
                newName: "Ticket");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_MovieId",
                table: "Ticket",
                newName: "IX_Ticket_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_CustomerId",
                table: "Ticket",
                newName: "IX_Ticket_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Customers_CustomerId",
                table: "Ticket",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Movies_MovieId",
                table: "Ticket",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }
    }
}
