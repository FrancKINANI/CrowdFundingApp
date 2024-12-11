using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrowdFundingApp.Migrations
{
    /// <inheritdoc />
    public partial class testIdentityUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "UserRewards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UzersId",
                table: "UserRewards",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UzersId",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Contributions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UzersId",
                table: "Contributions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRewards_UzersId",
                table: "UserRewards",
                column: "UzersId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UzersId",
                table: "Projects",
                column: "UzersId");

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_UzersId",
                table: "Contributions",
                column: "UzersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contributions_AspNetUsers_UzersId",
                table: "Contributions",
                column: "UzersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_UzersId",
                table: "Projects",
                column: "UzersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRewards_AspNetUsers_UzersId",
                table: "UserRewards",
                column: "UzersId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contributions_AspNetUsers_UzersId",
                table: "Contributions");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_UzersId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRewards_AspNetUsers_UzersId",
                table: "UserRewards");

            migrationBuilder.DropIndex(
                name: "IX_UserRewards_UzersId",
                table: "UserRewards");

            migrationBuilder.DropIndex(
                name: "IX_Projects_UzersId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Contributions_UzersId",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserRewards");

            migrationBuilder.DropColumn(
                name: "UzersId",
                table: "UserRewards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "UzersId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Contributions");

            migrationBuilder.DropColumn(
                name: "UzersId",
                table: "Contributions");
        }
    }
}
