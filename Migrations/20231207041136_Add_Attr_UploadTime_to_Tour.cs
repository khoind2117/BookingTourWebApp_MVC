using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingTourWebApp_MVC.Migrations
{
    public partial class Add_Attr_UploadTime_to_Tour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UploadTime",
                table: "Tour",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadTime",
                table: "Tour");
        }
    }
}
