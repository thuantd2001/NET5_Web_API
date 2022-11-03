using Microsoft.EntityFrameworkCore.Migrations;

namespace MyWebApiApp.Migrations
{
    public partial class AddType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdType",
                table: "Product",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    IdType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.IdType);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_IdType",
                table: "Product",
                column: "IdType");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Type_IdType",
                table: "Product",
                column: "IdType",
                principalTable: "Type",
                principalColumn: "IdType",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Type_IdType",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.DropIndex(
                name: "IX_Product_IdType",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "IdType",
                table: "Product");
        }
    }
}
