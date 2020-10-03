using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiveCRM.Migrations
{
    public partial class updatedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentID",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Dict",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(nullable: false),
                    Key = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dict", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProCustomer",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Uid = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Mobile = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProCustomer", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dict");

            migrationBuilder.DropTable(
                name: "ProCustomer");

            migrationBuilder.DropColumn(
                name: "ParentID",
                table: "AbpUsers");
        }
    }
}
