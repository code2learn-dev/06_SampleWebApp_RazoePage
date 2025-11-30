using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _06_WebApp_RazoePage.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangePhoneNumberAndNationalCodeToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Customers",
                type: "nvarchar(max)",
                precision: 11,
                scale: 0,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,0)",
                oldPrecision: 11);

            migrationBuilder.AlterColumn<string>(
                name: "NationalCode",
                table: "Customers",
                type: "nvarchar(max)",
                precision: 10,
                scale: 0,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,0)",
                oldPrecision: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PhoneNumber",
                table: "Customers",
                type: "decimal(11,0)",
                precision: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldPrecision: 11,
                oldScale: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "NationalCode",
                table: "Customers",
                type: "decimal(10,0)",
                precision: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldPrecision: 10,
                oldScale: 0);
        }
    }
}
