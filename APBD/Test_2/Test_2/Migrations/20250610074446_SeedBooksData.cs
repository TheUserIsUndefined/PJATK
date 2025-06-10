using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test_2.Migrations
{
    /// <inheritdoc />
    public partial class SeedBooksData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "IdBook", "IdPublishingHouse", "Name", "ReleaseDate" },
                values: new object[] { 1, 1, "Yes", new DateTime(2013, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "IdBook",
                keyValue: 1);
        }
    }
}
