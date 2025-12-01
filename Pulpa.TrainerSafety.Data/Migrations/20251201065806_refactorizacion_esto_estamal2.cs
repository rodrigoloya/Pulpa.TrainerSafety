using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pulpa.TrainerSafety.Data.Migrations
{
    /// <inheritdoc />
    public partial class refactorizacion_esto_estamal2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2025, 12, 1, 6, 54, 12, 445, DateTimeKind.Utc).AddTicks(7895));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "TrainerSafety",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2025, 12, 1, 6, 54, 12, 445, DateTimeKind.Utc).AddTicks(7895),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
