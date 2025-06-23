using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_DiscountTypes_DiscountTypeTypeId",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "DiscountTypeTypeId",
                table: "Discounts",
                newName: "DiscountTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Discounts_DiscountTypeTypeId",
                table: "Discounts",
                newName: "IX_Discounts_DiscountTypeId");

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientId", "Address", "Email", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Ul. Krakowska 15, 00-001 Warszawa", "jan.kowalski@email.com", "123456789" },
                    { 2, "Ul. Marszałkowska 100, 00-026 Warszawa", "kontakt@techcorp.pl", "987654321" },
                    { 3, "Ul. Nowy Świat 25, 00-029 Warszawa", "anna.nowak@gmail.com", "555123456" },
                    { 4, "Ul. Piękna 50, 00-672 Warszawa", "info@innovate.com.pl", "111222333" },
                    { 5, "Ul. Mokotowska 12, 00-640 Warszawa", "piotr.wisniewski@outlook.com", "444555666" }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "DiscountId", "Description", "DiscountTypeId", "EndDate", "Name", "PercentageValue", "StartDate" },
                values: new object[,]
                {
                    { 1, "Special discount for new year contracts", 1, new DateOnly(2025, 3, 31), "New Year 2025", 15.00m, new DateOnly(2025, 1, 1) },
                    { 2, "Discount for large volume purchases", 1, new DateOnly(2025, 12, 31), "Enterprise Package", 25.00m, new DateOnly(2025, 1, 1) },
                    { 3, "Loyalty discount for existing clients", 1, new DateOnly(2025, 12, 31), "Returning Customer", 10.00m, new DateOnly(2025, 1, 1) },
                    { 4, "Summer promotional discount", 1, new DateOnly(2025, 8, 31), "Summer Sale", 20.00m, new DateOnly(2025, 6, 1) },
                    { 5, "Educational institution discount", 2, new DateOnly(2025, 12, 31), "Student Special", 30.00m, new DateOnly(2025, 1, 1) }
                });

            migrationBuilder.UpdateData(
                table: "SoftwareCategories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "Name",
                value: "Business Management");

            migrationBuilder.UpdateData(
                table: "SoftwareCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "Name",
                value: "Accounting");

            migrationBuilder.InsertData(
                table: "SoftwareCategories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 3, "CRM" },
                    { 4, "Project Management" },
                    { 5, "Security" }
                });

            migrationBuilder.InsertData(
                table: "Softwares",
                columns: new[] { "SoftwareId", "CategoryId", "CurrentVersion", "Description", "Name", "SubscriptionCost", "UpfrontCostPerYear" },
                values: new object[,]
                {
                    { 1, 1, "3.2.1", "Complete business management solution", "BusinessPro", 150.00m, 1200.00m },
                    { 2, 2, "2.1.5", "Advanced accounting software", "AccounTech", 100.00m, null }
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "CompanyId", "ClientId", "Krs", "Name" },
                values: new object[,]
                {
                    { 1, 2, "0000123456", "TechCorp Sp. z o.o." },
                    { 2, 4, "0000987654", "Innovate Solutions" }
                });

            migrationBuilder.InsertData(
                table: "Individuals",
                columns: new[] { "IndividualId", "ClientId", "FirstName", "IsDeleted", "LastName", "Pesel" },
                values: new object[,]
                {
                    { 1, 1, "Jan", false, "Kowalski", "85010112345" },
                    { 2, 3, "Anna", false, "Nowak", "90052298765" },
                    { 3, 5, "Piotr", false, "Wiśniewski", "78121545678" }
                });

            migrationBuilder.InsertData(
                table: "Softwares",
                columns: new[] { "SoftwareId", "CategoryId", "CurrentVersion", "Description", "Name", "SubscriptionCost", "UpfrontCostPerYear" },
                values: new object[,]
                {
                    { 3, 3, "4.0.2", "Customer relationship management system", "ClientConnect", 250.00m, 2000.00m },
                    { 4, 4, "1.8.3", "Project and task management platform", "TaskMaster", 75.00m, 600.00m },
                    { 5, 5, "5.1.0", "Enterprise security management suite", "SecureGuard", 400.00m, 3000.00m }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_DiscountTypes_DiscountTypeId",
                table: "Discounts",
                column: "DiscountTypeId",
                principalTable: "DiscountTypes",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_DiscountTypes_DiscountTypeId",
                table: "Discounts");

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "CompanyId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "DiscountId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Individuals",
                keyColumn: "IndividualId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Individuals",
                keyColumn: "IndividualId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Individuals",
                keyColumn: "IndividualId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Softwares",
                keyColumn: "SoftwareId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "ClientId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SoftwareCategories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SoftwareCategories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SoftwareCategories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.RenameColumn(
                name: "DiscountTypeId",
                table: "Discounts",
                newName: "DiscountTypeTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Discounts_DiscountTypeId",
                table: "Discounts",
                newName: "IX_Discounts_DiscountTypeTypeId");

            migrationBuilder.UpdateData(
                table: "SoftwareCategories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "Name",
                value: "Finances");

            migrationBuilder.UpdateData(
                table: "SoftwareCategories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "Name",
                value: "Education");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_DiscountTypes_DiscountTypeTypeId",
                table: "Discounts",
                column: "DiscountTypeTypeId",
                principalTable: "DiscountTypes",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
