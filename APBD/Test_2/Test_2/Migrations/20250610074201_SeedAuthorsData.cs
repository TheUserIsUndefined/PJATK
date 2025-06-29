﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test_2.Migrations
{
    /// <inheritdoc />
    public partial class SeedAuthorsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "IdAuthor", "FirstName", "LastName" },
                values: new object[] { 1, "No", "One" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "IdAuthor",
                keyValue: 1);
        }
    }
}
