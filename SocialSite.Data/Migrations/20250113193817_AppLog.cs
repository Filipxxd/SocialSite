using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialSite.Data.Migrations
{
	/// <inheritdoc />
	public partial class AppLog : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateIndex(
				name: "IX_RefreshTokens_UserId",
				table: "RefreshTokens",
				column: "UserId");

			migrationBuilder.AddForeignKey(
				name: "FK_RefreshTokens_Users_UserId",
				table: "RefreshTokens",
				column: "UserId",
				principalTable: "Users",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.CreateTable(
				name: "AppLog",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Exception = table.Column<string>(type: "nvarchar(50)", nullable: true),
					Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserId = table.Column<string>(type: "nvarchar(50)", nullable: true),
					LogDate = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AppLog", x => x.Id);
				});
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(name: "AppLog");

			migrationBuilder.DropForeignKey(
				name: "FK_RefreshTokens_Users_UserId",
				table: "RefreshTokens");

			migrationBuilder.DropIndex(
				name: "IX_RefreshTokens_UserId",
				table: "RefreshTokens");
		}
	}
}
