using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoadState.DataAccessLayer.Migrations
{
    public partial class RemovedHomeLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "RegistrationDate", "UserName" },
                values: new object[] { "abcd", "123@gmail.com", new DateTime(2019, 7, 28, 11, 37, 37, 55, DateTimeKind.Local).AddTicks(3618), "dimasik" });

            migrationBuilder.InsertData(
                table: "BugReports",
                columns: new[] { "Id", "AuthorId", "Description", "Latitude", "Longitude", "PublishDate", "Rating", "State" },
                values: new object[] { 1, "abcd", "first bug report", 50.046199999999999, 36.315159999999999, new DateTime(2019, 7, 28, 11, 37, 37, 53, DateTimeKind.Local).AddTicks(2026), 1, "Low" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BugReports",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "abcd");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Users",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Users",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
