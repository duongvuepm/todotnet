using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace TodoApp.Migrations
{
    /// <inheritdoc />
    public partial class AddBoardEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "board_id",
                table: "state",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "board_id",
                table: "TodoItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "board",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "longtext", nullable: false),
                    description = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_board", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_state_board_id",
                table: "state",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_board_id",
                table: "TodoItems",
                column: "board_id");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_board_board_id",
                table: "TodoItems",
                column: "board_id",
                principalTable: "board",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_state_board_board_id",
                table: "state",
                column: "board_id",
                principalTable: "board",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_board_board_id",
                table: "TodoItems");

            migrationBuilder.DropForeignKey(
                name: "FK_state_board_board_id",
                table: "state");

            migrationBuilder.DropTable(
                name: "board");

            migrationBuilder.DropIndex(
                name: "IX_state_board_id",
                table: "state");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_board_id",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "board_id",
                table: "state");

            migrationBuilder.DropColumn(
                name: "board_id",
                table: "TodoItems");
        }
    }
}
