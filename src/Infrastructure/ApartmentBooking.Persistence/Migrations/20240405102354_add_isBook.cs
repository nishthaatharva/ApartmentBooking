using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_isBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBook",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("70edb255-7bf7-4597-d02c-08dc10d6610e"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 4, 5, 10, 23, 54, 50, DateTimeKind.Utc).AddTicks(7652), new DateTime(2024, 4, 5, 10, 23, 54, 50, DateTimeKind.Utc).AddTicks(7653), new DateTime(2024, 4, 5, 10, 23, 54, 50, DateTimeKind.Utc).AddTicks(7652) });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("7984dce3-39a8-4da3-bfac-08dc103b5c3f"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 4, 5, 10, 23, 54, 50, DateTimeKind.Utc).AddTicks(7617), new DateTime(2024, 4, 5, 10, 23, 54, 50, DateTimeKind.Utc).AddTicks(7618), new DateTime(2024, 4, 5, 10, 23, 54, 50, DateTimeKind.Utc).AddTicks(7617) });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("d0f230c1-a0bd-4085-7239-08dc02176302"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 4, 5, 10, 23, 54, 50, DateTimeKind.Utc).AddTicks(7604), new DateTime(2024, 4, 5, 10, 23, 54, 50, DateTimeKind.Utc).AddTicks(7607), new DateTime(2024, 4, 5, 10, 23, 54, 50, DateTimeKind.Utc).AddTicks(7606) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBook",
                table: "Bookings");

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
    }
}
