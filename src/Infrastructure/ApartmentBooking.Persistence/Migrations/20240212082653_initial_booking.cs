using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApartmentBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial_booking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookTill = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOnLease = table.Column<bool>(type: "bit", nullable: false),
                    ApartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("70edb255-7bf7-4597-d02c-08dc10d6610e"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 12, 8, 26, 52, 131, DateTimeKind.Utc).AddTicks(467), new DateTime(2024, 2, 12, 8, 26, 52, 131, DateTimeKind.Utc).AddTicks(468), new DateTime(2024, 2, 12, 8, 26, 52, 131, DateTimeKind.Utc).AddTicks(467) });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("7984dce3-39a8-4da3-bfac-08dc103b5c3f"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 12, 8, 26, 52, 131, DateTimeKind.Utc).AddTicks(463), new DateTime(2024, 2, 12, 8, 26, 52, 131, DateTimeKind.Utc).AddTicks(464), new DateTime(2024, 2, 12, 8, 26, 52, 131, DateTimeKind.Utc).AddTicks(463) });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("d0f230c1-a0bd-4085-7239-08dc02176302"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 12, 8, 26, 52, 131, DateTimeKind.Utc).AddTicks(442), new DateTime(2024, 2, 12, 8, 26, 52, 131, DateTimeKind.Utc).AddTicks(446), new DateTime(2024, 2, 12, 8, 26, 52, 131, DateTimeKind.Utc).AddTicks(445) });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ApartmentId",
                table: "Bookings",
                column: "ApartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("70edb255-7bf7-4597-d02c-08dc10d6610e"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4276), new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4277), new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4277) });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("7984dce3-39a8-4da3-bfac-08dc103b5c3f"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4275), new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4275), new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4275) });

            migrationBuilder.UpdateData(
                table: "Amenities",
                keyColumn: "Id",
                keyValue: new Guid("d0f230c1-a0bd-4085-7239-08dc02176302"),
                columns: new[] { "CreatedOn", "DeletedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4263), new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4264), new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4264) });
        }
    }
}
