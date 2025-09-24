using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AirlineBookingApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CouponSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "Id", "Code", "DiscountAmount", "DiscountPercent", "ExpiryDate", "IsActive" },
                values: new object[,]
                {
                    { 1, "WELCOME10", 10m, 10m, new DateTime(2025, 10, 24, 9, 32, 48, 667, DateTimeKind.Utc).AddTicks(94), true },
                    { 2, "WINTER10", 20m, 20m, new DateTime(2025, 10, 24, 9, 32, 48, 667, DateTimeKind.Utc).AddTicks(400), true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
