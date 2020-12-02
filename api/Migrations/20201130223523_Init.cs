using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AddressIndirection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressIndirection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressIndirection_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumberIndirection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumberIndirection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneNumberIndirection_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressIndirection_Address",
                table: "AddressIndirection",
                column: "Address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddressIndirection_CustomerId",
                table: "AddressIndirection",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumberIndirection_CustomerId",
                table: "PhoneNumberIndirection",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumberIndirection_PhoneNumber",
                table: "PhoneNumberIndirection",
                column: "PhoneNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressIndirection");

            migrationBuilder.DropTable(
                name: "PhoneNumberIndirection");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
