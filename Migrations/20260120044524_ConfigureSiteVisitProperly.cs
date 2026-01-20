using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Expense1.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureSiteVisitProperly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoolMembers_AspNetUsers_UserId",
                table: "PoolMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_SplitPools_AspNetUsers_CreatorId",
                table: "SplitPools");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "SplitPools",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "VisitDate",
                table: "SiteVisits",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "AmountPaid",
                table: "PoolMembers",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AmountDue",
                table: "PoolMembers",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_SiteVisits_VisitDate",
                table: "SiteVisits",
                column: "VisitDate",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PoolMembers_AspNetUsers_UserId",
                table: "PoolMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SplitPools_AspNetUsers_CreatorId",
                table: "SplitPools",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PoolMembers_AspNetUsers_UserId",
                table: "PoolMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_SplitPools_AspNetUsers_CreatorId",
                table: "SplitPools");

            migrationBuilder.DropIndex(
                name: "IX_SiteVisits_VisitDate",
                table: "SiteVisits");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "SplitPools",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "VisitDate",
                table: "SiteVisits",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AlterColumn<decimal>(
                name: "AmountPaid",
                table: "PoolMembers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<decimal>(
                name: "AmountDue",
                table: "PoolMembers",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_PoolMembers_AspNetUsers_UserId",
                table: "PoolMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SplitPools_AspNetUsers_CreatorId",
                table: "SplitPools",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
