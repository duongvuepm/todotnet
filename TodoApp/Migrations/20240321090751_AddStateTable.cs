using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace TodoApp.Migrations
{
    /// <inheritdoc />
    public partial class AddStateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TodoItems",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StateId",
                table: "TodoItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "state",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    previous_state_id = table.Column<long>(type: "bigint", nullable: true),
                    is_default = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    StateId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_state", x => x.id);
                    table.ForeignKey(
                        name: "FK_state_state_StateId",
                        column: x => x.StateId,
                        principalTable: "state",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_StateId",
                table: "TodoItems",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_state_StateId",
                table: "state",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_state_StateId",
                table: "TodoItems",
                column: "StateId",
                principalTable: "state",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_state_StateId",
                table: "TodoItems");

            migrationBuilder.DropTable(
                name: "state");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_StateId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "TodoItems");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TodoItems",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)");
        }
    }
}
