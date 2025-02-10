using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Simple.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddressModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSelected",
                table: "Addresses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSelected",
                table: "Addresses");
        }
    }
}
