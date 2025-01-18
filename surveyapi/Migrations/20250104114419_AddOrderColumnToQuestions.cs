using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace surveyapi.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderColumnToQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Questions");
        }
    }
}
