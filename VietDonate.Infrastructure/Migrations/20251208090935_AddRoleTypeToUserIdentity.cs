using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietDonate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleTypeToUserIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleType",
                table: "UserIdentities",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleType",
                table: "UserIdentities");
        }
    }
}
