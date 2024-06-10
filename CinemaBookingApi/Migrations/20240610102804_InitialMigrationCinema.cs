using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CinemaBookingApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationCinema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            //     name: "user",
            //     columns: table => new
            //     {
            //         id = table.Column<long>(type: "bigint", nullable: false)
            //             .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //         email = table.Column<string>(type: "text", nullable: true),
            //         password = table.Column<string>(type: "text", nullable: true),
            //         isConfirmed = table.Column<bool>(type: "boolean", nullable: true),
            //         refreshToken = table.Column<string>(type: "text", nullable: true),
            //         refreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_user", x => x.id);
            //     });

            migrationBuilder.CreateTable(
                name: "CinemaSeats",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    isBooked = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CinemaSeats", x => x.id);
                    table.ForeignKey(
                        name: "FK_CinemaSeats_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CinemaSeats_UserId",
                table: "CinemaSeats",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CinemaSeats");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
