using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Latest_ShopDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Card");

            migrationBuilder.DropTable(
                name: "FileEntries");

            migrationBuilder.DropTable(
                name: "OutboxMessage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BillingAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    BillingCity = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CardHolderName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CardNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Cvv = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ExpirationDate = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FileLocation = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    Published = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ScheduledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Card_CustomerId",
                table: "Card",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_Published",
                table: "OutboxMessage",
                column: "Published");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_ScheduledAt",
                table: "OutboxMessage",
                column: "ScheduledAt");
        }
    }
}
