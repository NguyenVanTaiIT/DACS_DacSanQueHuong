using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DACS.Migrations
{
    /// <inheritdoc />
    public partial class capnhatorderV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TinhTrangThanhToan",
                table: "CTDonHang");

            migrationBuilder.RenameColumn(
                name: "GhiChu",
                table: "DonHang",
                newName: "TinhTrangThanhToan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TinhTrangThanhToan",
                table: "DonHang",
                newName: "GhiChu");

            migrationBuilder.AddColumn<string>(
                name: "TinhTrangThanhToan",
                table: "CTDonHang",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
