using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Test_2.Migrations
{
    /// <inheritdoc />
    public partial class SeedPublishingHousesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PublishingHouses",
                columns: new[] { "IdPublishingHouse", "City", "Country", "Name" },
                values: new object[,]
                {
                    { 1, "New York", "USA", "Meow" },
                    { 2, "Moscow", "Russia", "Meow2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PublishingHouses",
                keyColumn: "IdPublishingHouse",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PublishingHouses",
                keyColumn: "IdPublishingHouse",
                keyValue: 2);
        }
    }
}
