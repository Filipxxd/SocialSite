using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialSite.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChatImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Images_Entity",
                table: "Images");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Images_Entity",
                table: "Images",
                sql: "[Entity] IN ('Post','Message','Profile','GroupChat')");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Chats_EntityId",
                table: "Images",
                column: "EntityId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Chats_EntityId",
                table: "Images");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Images_Entity",
                table: "Images");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Images_Entity",
                table: "Images",
                sql: "[Entity] IN ('Post','Message','Profile')");
        }
    }
}
