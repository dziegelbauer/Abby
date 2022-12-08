using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Abby.DataAccess.Migrations
{
    public partial class fixMenuItemModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_FoodTypes_FoodTypeIf",
                table: "MenuItems");

            migrationBuilder.DropIndex(
                name: "IX_MenuItems_FoodTypeIf",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "FoodTypeIf",
                table: "MenuItems");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_FoodTypes_FoodTypeId",
                table: "MenuItems",
                column: "FoodTypeId",
                principalTable: "FoodTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItems_FoodTypes_FoodTypeId",
                table: "MenuItems");

            migrationBuilder.AddColumn<int>(
                name: "FoodTypeIf",
                table: "MenuItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_FoodTypeIf",
                table: "MenuItems",
                column: "FoodTypeIf");

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItems_FoodTypes_FoodTypeIf",
                table: "MenuItems",
                column: "FoodTypeIf",
                principalTable: "FoodTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
