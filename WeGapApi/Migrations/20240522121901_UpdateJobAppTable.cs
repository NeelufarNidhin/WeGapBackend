using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeGapApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJobAppTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Jobs");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "JobApplications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "JobApplications");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
