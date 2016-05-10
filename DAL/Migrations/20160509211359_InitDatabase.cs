using System;
using Microsoft.Data.Entity.Migrations;

namespace DAL.Migrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("Category", table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(nullable: false),
                ParentId = table.Column<int>(nullable: true)
            },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                    table.ForeignKey("FK_Category_Category_ParentId", x => x.ParentId, "Category", "Id",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable("Currency", table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Code = table.Column<string>(nullable: false),
                Converter = table.Column<double>(nullable: false),
                Name = table.Column<string>(nullable: false),
                UpadeDate = table.Column<DateTime>(nullable: false)
            },
                constraints: table => { table.PrimaryKey("PK_Currency", x => x.Id); });
            migrationBuilder.CreateTable("User", table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Hash = table.Column<string>(nullable: true),
                IsPasswordSet = table.Column<bool>(nullable: false),
                Name = table.Column<string>(nullable: false),
                Salt = table.Column<string>(nullable: true)
            },
                constraints: table => { table.PrimaryKey("PK_User", x => x.Id); });
            migrationBuilder.CreateTable("Asset", table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Name = table.Column<string>(nullable: false),
                Type = table.Column<int>(nullable: false),
                UserId = table.Column<int>(nullable: false)
            },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asset", x => x.Id);
                    table.ForeignKey("FK_Asset_User_UserId", x => x.UserId, "User", "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            migrationBuilder.CreateTable("Transaction", table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                AssetId = table.Column<int>(nullable: false),
                Comment = table.Column<string>(nullable: true),
                Cost = table.Column<double>(nullable: false),
                CurrencyId = table.Column<int>(nullable: false),
                Date = table.Column<DateTime>(nullable: false),
                ProductId = table.Column<int>(nullable: false),
                Type = table.Column<int>(nullable: false)
            },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey("FK_Transaction_Asset_AssetId", x => x.AssetId, "Asset", "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_Transaction_Currency_CurrencyId", x => x.CurrencyId, "Currency", "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey("FK_Transaction_Category_ProductId", x => x.ProductId, "Category", "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("Transaction");
            migrationBuilder.DropTable("Asset");
            migrationBuilder.DropTable("Currency");
            migrationBuilder.DropTable("Category");
            migrationBuilder.DropTable("User");
        }
    }
}