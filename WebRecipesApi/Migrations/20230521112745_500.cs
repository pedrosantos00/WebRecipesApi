using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebRecipesApi.Migrations
{
    /// <inheritdoc />
    public partial class _500 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Aprooved",
                table: "Recipes",
                newName: "Approved");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Approved",
                table: "Recipes",
                newName: "Aprooved");
        }
    }
}
