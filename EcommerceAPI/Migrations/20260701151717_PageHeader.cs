using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class PageHeader : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PageHeaders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PageTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PageQuickLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Route = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    PageHeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageQuickLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageQuickLinks_PageHeaders_PageHeaderId",
                        column: x => x.PageHeaderId,
                        principalTable: "PageHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageHeaders_PageKey",
                table: "PageHeaders",
                column: "PageKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PageQuickLinks_PageHeaderId",
                table: "PageQuickLinks",
                column: "PageHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageQuickLinks");

            migrationBuilder.DropTable(
                name: "PageHeaders");
        }
    }
}
