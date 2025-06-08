using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace DiplomaApi.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCoordsToEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<NpgsqlPoint>(
                name: "Coords",
                table: "events",
                type: "point",
                nullable: false,
                defaultValue: new NpgsqlTypes.NpgsqlPoint(0.0, 0.0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coords",
                table: "events");
        }
    }
}
