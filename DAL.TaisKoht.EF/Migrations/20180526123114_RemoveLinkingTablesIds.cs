using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace DAL.TaisKoht.EF.Migrations
{
    public partial class RemoveLinkingTablesIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestaurantUserId",
                table: "RestaurantUsers");

            migrationBuilder.DropColumn(
                name: "MenuDishId",
                table: "MenuDishes");

            migrationBuilder.DropColumn(
                name: "DishIngredientId",
                table: "DishIngredients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantUserId",
                table: "RestaurantUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MenuDishId",
                table: "MenuDishes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DishIngredientId",
                table: "DishIngredients",
                nullable: false,
                defaultValue: 0);
        }
    }
}
