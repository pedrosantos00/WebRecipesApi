using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRecipesApi.Migrations
{
    /// <inheritdoc />
    public partial class te : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RateAudit_Recipes_RecipeId",
                table: "RateAudit");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "RateAudit",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RateAudit_Recipes_RecipeId",
                table: "RateAudit",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RateAudit_Recipes_RecipeId",
                table: "RateAudit");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "RateAudit",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RateAudit_Recipes_RecipeId",
                table: "RateAudit",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id");
        }
    }
}
