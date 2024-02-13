using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApartmentBooking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_amenities_list : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amenities",
                table: "Apartments");

            migrationBuilder.CreateTable(
                name: "Amenities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_Amenities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentAmenitiesAssociations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmenitiesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_ApartmentAmenitiesAssociations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApartmentAmenitiesAssociations_Amenities_AmenitiesId",
                        column: x => x.AmenitiesId,
                        principalTable: "Amenities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApartmentAmenitiesAssociations_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Amenities",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DeletedBy", "DeletedOn", "IsDeleted", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { new Guid("70edb255-7bf7-4597-d02c-08dc10d6610e"), null, new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4276), null, new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4277), false, null, new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4277), "Garden" },
                    { new Guid("7984dce3-39a8-4da3-bfac-08dc103b5c3f"), null, new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4275), null, new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4275), false, null, new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4275), "Pool" },
                    { new Guid("d0f230c1-a0bd-4085-7239-08dc02176302"), null, new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4263), null, new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4264), false, null, new DateTime(2024, 2, 6, 9, 15, 7, 818, DateTimeKind.Utc).AddTicks(4264), "Gym" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentAmenitiesAssociations_AmenitiesId",
                table: "ApartmentAmenitiesAssociations",
                column: "AmenitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentAmenitiesAssociations_ApartmentId",
                table: "ApartmentAmenitiesAssociations",
                column: "ApartmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentAmenitiesAssociations");

            migrationBuilder.DropTable(
                name: "Amenities");

            migrationBuilder.AddColumn<int>(
                name: "Amenities",
                table: "Apartments",
                type: "int",
                nullable: true);
        }
    }
}
