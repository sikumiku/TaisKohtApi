using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DAL.TaisKoht.EF.Migrations
{
    public partial class RemoveMenuFromRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RatingLogs_Menus_MenuId",
                table: "RatingLogs");

            migrationBuilder.DropIndex(
                name: "IX_RatingLogs_MenuId",
                table: "RatingLogs");

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "RatingLogs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MenuId",
                table: "RatingLogs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RatingLogs_MenuId",
                table: "RatingLogs",
                column: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_RatingLogs_Menus_MenuId",
                table: "RatingLogs",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
