using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sso.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ClientScopeManyToManyAndUserPermissionGrantedByIsOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scope_Client_ClientId",
                table: "Scope");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_User_GrantedById",
                table: "UserPermission");

            migrationBuilder.DropIndex(
                name: "IX_Scope_ClientId",
                table: "Scope");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Scope");

            migrationBuilder.AlterColumn<int>(
                name: "GrantedById",
                table: "UserPermission",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(254)",
                oldMaxLength: 254);

            migrationBuilder.CreateTable(
                name: "ClientScope",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    ScopesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientScope", x => new { x.ClientId, x.ScopesId });
                    table.ForeignKey(
                        name: "FK_ClientScope_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientScope_Scope_ScopesId",
                        column: x => x.ScopesId,
                        principalTable: "Scope",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientScope_ScopesId",
                table: "ClientScope",
                column: "ScopesId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_User_GrantedById",
                table: "UserPermission",
                column: "GrantedById",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermission_User_GrantedById",
                table: "UserPermission");

            migrationBuilder.DropTable(
                name: "ClientScope");

            migrationBuilder.AlterColumn<int>(
                name: "GrantedById",
                table: "UserPermission",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "User",
                type: "character varying(254)",
                maxLength: 254,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Scope",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scope_ClientId",
                table: "Scope",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scope_Client_ClientId",
                table: "Scope",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermission_User_GrantedById",
                table: "UserPermission",
                column: "GrantedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
