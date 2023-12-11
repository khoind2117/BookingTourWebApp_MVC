using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingTourWebApp_MVC.Migrations
{
    public partial class Add_Attr_Image_to_Tour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Tour",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Tour");
        }
    }
}
