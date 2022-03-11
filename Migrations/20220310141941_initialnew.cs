using Microsoft.EntityFrameworkCore.Migrations;

namespace ArchimydesWeb.Migrations
{
    public partial class initialnew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SYS_Users_SYS_Role_RoleID",
                table: "SYS_Users");

            migrationBuilder.DropIndex(
                name: "IX_SYS_Users_RoleID",
                table: "SYS_Users");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SYS_Role");

            migrationBuilder.AlterColumn<int>(
                name: "RoleID",
                table: "SYS_Users",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RoleID",
                table: "SYS_Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "SYS_Role",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_RoleID",
                table: "SYS_Users",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_SYS_Users_SYS_Role_RoleID",
                table: "SYS_Users",
                column: "RoleID",
                principalTable: "SYS_Role",
                principalColumn: "RoleID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
