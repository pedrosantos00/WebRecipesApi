using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRecipesApi.Migrations
{
    /// <inheritdoc />
    public partial class _120 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavoriteRecipes",
                table: "UserFavoriteRecipes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserFavoriteRecipes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavoriteRecipes",
                table: "UserFavoriteRecipes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteRecipes_UserId",
                table: "UserFavoriteRecipes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavoriteRecipes",
                table: "UserFavoriteRecipes");

            migrationBuilder.DropIndex(
                name: "IX_UserFavoriteRecipes_UserId",
                table: "UserFavoriteRecipes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserFavoriteRecipes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavoriteRecipes",
                table: "UserFavoriteRecipes",
                columns: new[] { "UserId", "RecipeId" });
        }
    }
}
