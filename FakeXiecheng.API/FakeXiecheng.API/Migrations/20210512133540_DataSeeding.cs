using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FakeXiecheng.API.Migrations
{
    public partial class DataSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TouristRoutes",
                columns: new[] { "Id", "CreteTime", "DepartureTime", "Description", "DiscountPrecent", "Features", "Fees", "Notes", "OriginalPrice", "Title", "UpdateTime" },
                values: new object[] { new Guid("d8d1c302-6d7d-41fb-937a-e3795258bf57"), new DateTime(2021, 5, 12, 13, 35, 39, 763, DateTimeKind.Utc).AddTicks(2308), null, "shuoming", null, null, null, null, 0m, "ceshititle", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TouristRoutes",
                keyColumn: "Id",
                keyValue: new Guid("d8d1c302-6d7d-41fb-937a-e3795258bf57"));
        }
    }
}
