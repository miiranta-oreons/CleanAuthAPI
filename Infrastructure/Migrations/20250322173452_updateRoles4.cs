using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRoles4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EntityRole",
                table: "EntityRole");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "EntityRole",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntityRole",
                table: "EntityRole",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EntityRole",
                table: "EntityRole");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "EntityRole");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EntityRole",
                table: "EntityRole",
                column: "Name");
        }
    }
}
