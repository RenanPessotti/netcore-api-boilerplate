using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Boilerplate.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Persons",
                table => new
                {
                    Id = table.Column<Guid>("uniqueidentifier", nullable: false),
                    Name = table.Column<string>("nvarchar(max)", nullable: false),
                    Nickname = table.Column<string>("nvarchar(max)", nullable: true),
                    Job = table.Column<string>("nvarchar(max)", nullable: true),
                    Age = table.Column<int>("int", nullable: true),
                    Sex = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Persons");
        }
    }
}
