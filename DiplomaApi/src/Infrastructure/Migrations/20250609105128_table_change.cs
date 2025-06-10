using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class table_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_events_Sensors_SensorId",
                table: "events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_events",
                table: "events");

            migrationBuilder.RenameTable(
                name: "events",
                newName: "Events");

            migrationBuilder.RenameIndex(
                name: "IX_events_SensorId",
                table: "Events",
                newName: "IX_Events_SensorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Events",
                table: "Events",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Sensors_SensorId",
                table: "Events",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Sensors_SensorId",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Events",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Events",
                newName: "events");

            migrationBuilder.RenameIndex(
                name: "IX_Events_SensorId",
                table: "events",
                newName: "IX_events_SensorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_events",
                table: "events",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_events_Sensors_SensorId",
                table: "events",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
