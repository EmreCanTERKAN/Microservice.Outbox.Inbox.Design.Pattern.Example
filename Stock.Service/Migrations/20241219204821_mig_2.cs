using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Service.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderInBoxes",
                table: "OrderInBoxes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrderInBoxes");

            migrationBuilder.AddColumn<Guid>(
                name: "IdempotentToken",
                table: "OrderInBoxes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderInBoxes",
                table: "OrderInBoxes",
                column: "IdempotentToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderInBoxes",
                table: "OrderInBoxes");

            migrationBuilder.DropColumn(
                name: "IdempotentToken",
                table: "OrderInBoxes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderInBoxes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderInBoxes",
                table: "OrderInBoxes",
                column: "Id");
        }
    }
}
