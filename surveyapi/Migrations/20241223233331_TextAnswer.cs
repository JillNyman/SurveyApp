using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace surveyapi.Migrations
{
    /// <inheritdoc />
    public partial class TextAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MultipleChoiceOptions",
                table: "Questions",
                newName: "TextAnswer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TextAnswer",
                table: "Questions",
                newName: "MultipleChoiceOptions");
        }
    }
}
