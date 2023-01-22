using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelList.API.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaultRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15d456ca-c729-4917-af74-734e5aa200bd", "f1ff06ab-7e4d-4c66-a036-5056db31788c", "Administrator", "ADMINISTRATOR" },
                    { "2c913c79-d778-4f2c-acff-e9980159eae7", "0205093a-4ee6-4d5b-b529-7d3cc033b782", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15d456ca-c729-4917-af74-734e5aa200bd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c913c79-d778-4f2c-acff-e9980159eae7");
        }
    }
}
