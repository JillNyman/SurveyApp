using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace surveyapi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumericAnswer",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TextAnswer",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Answers");

            migrationBuilder.AddColumn<int>(
                name: "NumericAnswer",
                table: "Answers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SelectedOptions",
                table: "Answers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextAnswer",
                table: "Answers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Answers",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumericAnswer",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "SelectedOptions",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "TextAnswer",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Answers");

            migrationBuilder.AddColumn<int>(
                name: "NumericAnswer",
                table: "Questions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TextAnswer",
                table: "Questions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Answers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
