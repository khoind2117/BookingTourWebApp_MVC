using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingTourWebApp_MVC.Migrations
{
    public partial class Add_Attr_Discount_to_Tour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Discount",
                table: "Tour",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Tour");
        }
    }
}
