using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Threads.Migrations
{
    public partial class InitialiCreated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThreadTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThreadTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Threads",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    IsInternal = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TypeId = table.Column<Guid>(nullable: false),
                    Pitch = table.Column<decimal>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Threads_ThreadTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ThreadTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Threads_TypeId",
                table: "Threads",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Threads");

            migrationBuilder.DropTable(
                name: "ThreadTypes");
        }
    }
}
