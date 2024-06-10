using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class UserPropUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "username",
                table: "user");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "user",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "email",
                table: "user");

            migrationBuilder.AddColumn<string>(
                name: "username",
                table: "user",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true);
        }
    }
}
