using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Context.Component.Migrations
{
    public partial class Component : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryRelation",
                columns: table => new
                {
                    Category = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Extension = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryRelation", x => x.Category);
                });

            migrationBuilder.CreateTable(
                name: "ComponentDescriptor",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Category = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentDescriptor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComponentInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Enabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ComponentRelation",
                columns: table => new
                {
                    ChildId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    ChildCategory = table.Column<int>(nullable: false),
                    ParentCategory = table.Column<int>(nullable: false),
                    Locked = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    PositionColumn = table.Column<int>(nullable: false),
                    PositionRow = table.Column<int>(nullable: false),
                    PositionIndex = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentRelation", x => x.ChildId);
                });

            migrationBuilder.CreateTable(
                name: "ExtensionGeometry",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PositionCol = table.Column<int>(nullable: false),
                    PositionRow = table.Column<int>(nullable: false),
                    SizeCols = table.Column<int>(nullable: false),
                    SizeRows = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionGeometry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtensionImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    Distorted = table.Column<bool>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionImage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtensionLayout",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Style = table.Column<string>(nullable: true),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionLayout", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtensionNode",
                columns: table => new
                {
                    ChildId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    Position = table.Column<int>(nullable: false),
                    Locked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionNode", x => x.ChildId);
                });

            migrationBuilder.CreateTable(
                name: "ExtensionText",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Header = table.Column<string>(nullable: true),
                    Footer = table.Column<string>(nullable: true),
                    Paragraph = table.Column<string>(nullable: true),
                    HeaderVisibility = table.Column<string>(nullable: true),
                    FooterVisibility = table.Column<string>(nullable: true),
                    HtlmHeader = table.Column<string>(nullable: true),
                    HtmlFooter = table.Column<string>(nullable: true),
                    HtmlParagraph = table.Column<string>(nullable: true),
                    Caption = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ExternalLink = table.Column<string>(nullable: true),
                    Commit = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtensionText", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryRelation");

            migrationBuilder.DropTable(
                name: "ComponentDescriptor");

            migrationBuilder.DropTable(
                name: "ComponentInfo");

            migrationBuilder.DropTable(
                name: "ComponentRelation");

            migrationBuilder.DropTable(
                name: "ExtensionGeometry");

            migrationBuilder.DropTable(
                name: "ExtensionImage");

            migrationBuilder.DropTable(
                name: "ExtensionLayout");

            migrationBuilder.DropTable(
                name: "ExtensionNode");

            migrationBuilder.DropTable(
                name: "ExtensionText");
        }
    }
}
