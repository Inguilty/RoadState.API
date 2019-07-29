using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RoadState.DataAccessLayer.Migrations
{
    public partial class initialSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Latitude", "Longitude", "RegistrationDate", "UserName" },
                values: new object[] { "abcd", "123@gmail.com", 34.0, 55.0, new DateTime(2019, 7, 25, 17, 41, 44, 292, DateTimeKind.Local).AddTicks(8054), "dimasik" });

            migrationBuilder.InsertData(
                table: "BugReports",
                columns: new[] { "Id", "AuthorId", "Description", "Latitude", "Longitude", "PublishDate", "Rating", "State" },
                values: new object[] { 1, "abcd", "first bug report", 50.046199999999999, 36.315159999999999, new DateTime(2019, 7, 25, 17, 41, 44, 288, DateTimeKind.Local).AddTicks(134), 1, "Low" });
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
        }
    }
}
