using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthApi.Migrations
{
    /// <inheritdoc />
    public partial class UserRefreshTokenUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "refreshToken",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "refreshTokenExpiryTime",
                table: "user",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refreshToken",
                table: "user");

            migrationBuilder.DropColumn(
                name: "refreshTokenExpiryTime",
                table: "user");
        }
    }
}
