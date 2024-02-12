using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial_lease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeaseDuration",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("70edb255-7bf7-4597-d02c-08dc10d6610e"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 12, 12, 38, 7, 350, DateTimeKind.Utc).AddTicks(1817), new DateTime(2024, 2, 12, 12, 38, 7, 350, DateTimeKind.Utc).AddTicks(1817), new DateTime(2024, 2, 12, 12, 38, 7, 350, DateTimeKind.Utc).AddTicks(1817) });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("7984dce3-39a8-4da3-bfac-08dc103b5c3f"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 12, 12, 38, 7, 350, DateTimeKind.Utc).AddTicks(1814), new DateTime(2024, 2, 12, 12, 38, 7, 350, DateTimeKind.Utc).AddTicks(1815), new DateTime(2024, 2, 12, 12, 38, 7, 350, DateTimeKind.Utc).AddTicks(1814) });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("d0f230c1-a0bd-4085-7239-08dc02176302"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 12, 12, 38, 7, 350, DateTimeKind.Utc).AddTicks(1794), new DateTime(2024, 2, 12, 12, 38, 7, 350, DateTimeKind.Utc).AddTicks(1798), new DateTime(2024, 2, 12, 12, 38, 7, 350, DateTimeKind.Utc).AddTicks(1797) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaseDuration",
                table: "Bookings");

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("70edb255-7bf7-4597-d02c-08dc10d6610e"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 12, 12, 35, 26, 224, DateTimeKind.Utc).AddTicks(8329), new DateTime(2024, 2, 12, 12, 35, 26, 224, DateTimeKind.Utc).AddTicks(8330), new DateTime(2024, 2, 12, 12, 35, 26, 224, DateTimeKind.Utc).AddTicks(8329) });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("7984dce3-39a8-4da3-bfac-08dc103b5c3f"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 12, 12, 35, 26, 224, DateTimeKind.Utc).AddTicks(8325), new DateTime(2024, 2, 12, 12, 35, 26, 224, DateTimeKind.Utc).AddTicks(8326), new DateTime(2024, 2, 12, 12, 35, 26, 224, DateTimeKind.Utc).AddTicks(8325) });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("d0f230c1-a0bd-4085-7239-08dc02176302"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 12, 12, 35, 26, 224, DateTimeKind.Utc).AddTicks(8304), new DateTime(2024, 2, 12, 12, 35, 26, 224, DateTimeKind.Utc).AddTicks(8309), new DateTime(2024, 2, 12, 12, 35, 26, 224, DateTimeKind.Utc).AddTicks(8308) });
        }
    }
}
