using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApp.Migrations
{
    /// <inheritdoc />
    public partial class AddTransitionRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_state_state_StateId",
                table: "state");

            migrationBuilder.DropIndex(
                name: "IX_state_StateId",
                table: "state");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "state");

            migrationBuilder.DropColumn(
                name: "previous_state_id",
                table: "state");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StateId",
                table: "state",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "previous_state_id",
                table: "state",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_state_StateId",
                table: "state",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_state_state_StateId",
                table: "state",
                column: "StateId",
                principalTable: "state",
                principalColumn: "id");
        }
    }
}
